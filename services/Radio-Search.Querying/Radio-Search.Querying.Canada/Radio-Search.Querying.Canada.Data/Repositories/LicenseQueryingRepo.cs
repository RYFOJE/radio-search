using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Radio_Search.Canada.Models.License;
using Radio_Search.Querying.Canada.Data.Repositories.Interfaces;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Data.Repositories
{
    public class LicenseQueryingRepo : ILicenseQueryingRepo
    {
        private CanadaContext _context;
        private static GeometryFactory GeometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        public LicenseQueryingRepo(CanadaContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<(List<LicenseRecord>, bool)> QueryLicenses(LicenseQueryOptions query)
        {
            var licenses = _context.LicenseRecords.AsQueryable();

            if (query.LocationInformation != null)
            {
                var point = GeometryFactory.CreatePoint(
                    new Coordinate(query.LocationInformation.Longitude, query.LocationInformation.Latitude)
                );
                licenses = licenses.Where(lr =>
                    lr.Location != null &&
                    lr.Location.IsWithinDistance(point, query.LocationInformation.RadiusInMeters)
                );
            }

            if (!string.IsNullOrEmpty(query.FuzzySearchName))
            {
                licenses = licenses
                    .Where(lr => EF.Functions.TrigramsSimilarity(lr.LicenseeName!, query.FuzzySearchName) > 0.3)
                    .OrderByDescending(lr => EF.Functions.TrigramsSimilarity(lr.LicenseeName!, query.FuzzySearchName));
            }

            if (!string.IsNullOrWhiteSpace(query.Callsign))
                licenses = licenses.Where(lr => EF.Functions.ILike(lr.CallSign!, query.Callsign));

            if (!string.IsNullOrWhiteSpace(query.AccountNumber))
                licenses = licenses.Where(lr => EF.Functions.ILike(lr.AccountNumber!, query.AccountNumber));

            if (query.frequencyMin.HasValue)
                licenses = licenses.Where(lr => lr.FrequencyMHz >= (decimal)query.frequencyMin.Value);

            if (query.frequencyMax.HasValue)
                licenses = licenses.Where(lr => lr.FrequencyMHz <= (decimal)query.frequencyMax.Value);

            if (!string.IsNullOrWhiteSpace(query.StationFunction))
                licenses = licenses.Where(lr => EF.Functions.ILike(lr.StationFunctionID!, query.StationFunction!));

            if (query.AnalogDigital.HasValue)
                licenses = licenses.Where(lr => lr.AnalogDigitalID == query.AnalogDigital.Value);

            if (query.LastSeenCursor.HasValue)
                licenses = licenses.Where(lr => lr.CanadaLicenseRecordID >= query.LastSeenCursor.Value);

            licenses = licenses.OrderBy(lr => lr.CanadaLicenseRecordID);

            var result = await licenses
                .Take(query.PageSize + 1)
                .AsNoTracking()
                .ToListAsync();

            return (result.Take(query.PageSize).ToList(), result.Count == query.PageSize + 1);
        }
    }
}
