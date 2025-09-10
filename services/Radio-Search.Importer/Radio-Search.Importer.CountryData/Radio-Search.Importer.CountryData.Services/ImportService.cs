using Microsoft.Data.SqlClient;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Search.Importer.CountryData.Services
{
    public class ImportService
    {
        private readonly Stream blobStream; // Add this field to resolve 'blobStream' error
        private readonly string connectionString; // Add this field to resolve 'connectionString' error

        public ImportService(Stream blobStream, string connectionString)
        {
            this.blobStream = blobStream ?? throw new ArgumentNullException(nameof(blobStream));
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task TestImport()
        {
            // Expecting blobStream to be a ZIP archive that contains the .shp/.dbf/.shx files.
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                // Ensure stream is positioned at start if seekable
                if (blobStream.CanSeek)
                    blobStream.Position = 0;

                // Extract ZIP to temp directory
                using (var zip = new ZipArchive(blobStream, ZipArchiveMode.Read, leaveOpen: false))
                {
                    zip.ExtractToDirectory(tempDir);
                }

                // Find the .shp file
                var shpPath = Directory.EnumerateFiles(tempDir, "*.shp", SearchOption.AllDirectories).FirstOrDefault();
                if (shpPath == null)
                    throw new InvalidOperationException("No .shp file found in the provided archive.");

                // Create a geometry factory compatible with the NetTopologySuite ESRI shapefile reader
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                // Use the ESRI shapefile reader implementation which accepts an NTS GeometryFactory
                using var reader = new NetTopologySuite.IO.Esri.Shapefile.ShapefileDataReader(shpPath, geometryFactory);

                using var conn = new SqlConnection(connectionString);
                conn.Open();

                while (reader.Read())
                {
                    var geom = reader.Geometry; // Polygon or MultiPolygon
                    string wkt = geom.AsText();

                    string countryName = reader.GetString(reader.GetOrdinal("NAME"));

                    using var cmd = new SqlCommand(
                        "INSERT INTO Countries (CountryName, Boundary) VALUES (@name, geography::STGeomFromText(@wkt, 4326))", conn);
                    cmd.Parameters.AddWithValue("@name", countryName);
                    cmd.Parameters.AddWithValue("@wkt", wkt);
                    cmd.ExecuteNonQuery();
                }

                await Task.CompletedTask;
            }
            finally
            {
                // Cleanup temp directory
                try
                {
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, recursive: true);
                }
                catch
                {
                    // Swallow cleanup exceptions; optionally log if you have logging available
                }
            }
        }
    }
}#