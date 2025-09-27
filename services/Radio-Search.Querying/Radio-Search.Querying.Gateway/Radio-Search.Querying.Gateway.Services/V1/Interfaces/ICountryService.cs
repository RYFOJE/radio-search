using NetTopologySuite.Features;
using NetTopologySuite.Index.Strtree;

namespace Radio_Search.Querying.Gateway.Services.V1.Interfaces
{
    public interface ICountryService
    {
        /// <summary>
        /// Function to get overlapping countries from latitude longitude and a circle from those coordinates
        /// </summary>
        /// <param name="lat">Latitude of the point to evaluate</param>
        /// <param name="lon">Longitude of the point to evaluate</param>
        /// <param name="radius">The search radius of the point</param>
        /// <returns></returns>
        List<string> GetOverlappingCountries(double lat, double lon, double radius);


        /// <summary>
        /// Refreshes all the features contained within the Features Data Structure
        /// </summary>
        Task RefreshAllFeaturesAsync();

        /// <summary>
        /// Gets all the country features from the given data store
        /// </summary>
        /// <returns></returns>
        Task<STRtree<Feature>> GetFeaturesFromDataSourceAsync();

    }
}
