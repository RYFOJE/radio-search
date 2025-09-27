using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;
using NetTopologySuite.Index.Strtree;
using NetTopologySuite.IO;
using Radio_Search.Querying.Gateway.Services.V1.Interfaces;
using Radio_Search.Utils.BlobStorage.Interfaces;

namespace Radio_Search.Querying.Gateway.Services.V1
{

    /// <summary>
    /// NOTE: This is meant to be instantiated as a singleton
    /// </summary>
    public class CountryService : ICountryService, IHealthCheck
    {
        private STRtree<Feature> _indexedCountries = new();

        private readonly IConfiguration _config;
        private readonly IBlobStorageService _blobStorage;
        private readonly ILogger<CountryService> _logger;


        private const string COUNTRY_FILE_LOCATION = "CountryDataFileLocation";
        private const string LOCAL_FILE_LOCATION = "CountryData";

        public CountryService(
            IConfiguration config,
            IBlobStorageService blobStorageService,
            ILogger<CountryService> logger
            )
        {
            _config = config;
            _blobStorage = blobStorageService;
            _logger = logger;
        }
        
        public List<string> GetOverlappingCountries(double lat, double lon, double radiusKm)
        {
            var centerPoint = new Point(lon, lat);
            double radiusDegrees = KmToRadiusDegrees(radiusKm);
            var circle = centerPoint.Buffer(radiusDegrees);

            var candidates = _indexedCountries.Query(circle.EnvelopeInternal);
            var countries = new List<string>();

            foreach (var feature in candidates)
            {
                if (feature.Geometry.Intersects(circle))
                {
                    var name = feature.Attributes["ADMIN"]?.ToString();
                    if (!string.IsNullOrEmpty(name))
                        countries.Add(name);
                }
            }

            return countries;
        }

        public async Task RefreshAllFeaturesAsync()
        {
            _logger.LogInformation("Refreshing all map features.");
            var loadedFeatures = await GetFeaturesFromDataSourceAsync();

            _indexedCountries = loadedFeatures;

            _logger.LogInformation("Finished refreshing all map features.");
        }

        public async Task<STRtree<Feature>> GetFeaturesFromDataSourceAsync()
        {
            var blobName = _config[COUNTRY_FILE_LOCATION]
                ?? throw new NullReferenceException("Config value missing for blob location.");

            var shpFileLocation = await DownloadCountryDataFolder(blobName);


            var validFeatures = new List<Feature>();
            var geometryFactory = new GeometryFactory();

            using var reader = new ShapefileDataReader(shpFileLocation, geometryFactory);
            var dbfHeader = reader.DbaseHeader;

            while (reader.Read())
            {
                try
                {
                    var rawGeometry = reader.Geometry;
                    if (rawGeometry == null || rawGeometry.IsEmpty)
                        continue;

                    var geometry = GeometryFixer.Fix(rawGeometry);
                    if (!geometry.IsValid) continue;

                    var attributes = new AttributesTable();
                    for (int i = 0; i < dbfHeader.NumFields; i++)
                    {
                        var field = dbfHeader.Fields[i];
                        attributes.Add(field.Name, reader.GetValue(i + 1));
                    }

                    validFeatures.Add(new Feature(geometry, attributes));
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "Failed to load portion of shp file.");
                }
            }

            // Build the indexed values
            var index = new STRtree<Feature>();
            foreach (var feature in validFeatures)
            {
                index.Insert(feature.Geometry.EnvelopeInternal, feature);
            }
            index.Build();
            return index;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_indexedCountries == null || _indexedCountries.Count == 0)
                return Task.FromResult(HealthCheckResult.Unhealthy("Country Features not loaded"));

            return Task.FromResult(HealthCheckResult.Healthy("Country Features loaded"));
        }

        /// <summary>
        /// Download from the blob containing all the files and saves the files to a temp directory
        /// </summary>
        /// <returns>String corresponding to the location of the shp file location on drive</returns>
        public async Task<string> DownloadCountryDataFolder(string blobParentDirectory)
        {
            var files = await _blobStorage.ListBlobsForDirectory(blobParentDirectory);
            var tempPath = Path.Combine(Path.GetTempPath(), LOCAL_FILE_LOCATION);
            string? shpLocation = null;

            if (!files.Any())
                throw new InvalidOperationException("No files found in ShapeFile Directory.");

            foreach(var file in files)
            {
                var fileName = Path.GetFileName(file);

                var blobFileStream = await _blobStorage.DownloadAsync(file);

                // Save to disk temp folder
                var downloadPath = Path.Combine(tempPath, fileName);
                using var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await blobFileStream.CopyToAsync(fileStream);

                if(string.Equals(Path.GetExtension(file), ".shp", StringComparison.InvariantCultureIgnoreCase))
                {
                    shpLocation = downloadPath;
                }
            }

            if (string.IsNullOrWhiteSpace(shpLocation))
                throw new InvalidOperationException("Could not find .shp file while downloading.");


            return shpLocation;
        }


        public static double KmToRadiusDegrees(double km)
        {
            const double earthCircumferenceKm = 40075;
            return (km / earthCircumferenceKm) * 360;
        }
    }
}
