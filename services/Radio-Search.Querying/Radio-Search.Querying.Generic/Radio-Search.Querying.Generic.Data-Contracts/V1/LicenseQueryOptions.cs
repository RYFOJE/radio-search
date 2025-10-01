namespace Radio_Search.Querying.Generic.Data_Contracts.V1
{
    public class LicenseQueryOptions : QueryOptions
    {
        /// <summary>
        /// Fuzzy search on name. This name comes from any text field containing registration name, owner name, or other name fields.
        /// </summary>
        public string? FuzzySearchName { get; set; }

        /// <summary>
        /// Gets or sets the callsign associated with the Radio License.
        /// </summary>
        public string? Callsign { get; set; }

        /// <summary>
        /// The account number associated with the Radio License. This will usually be the licensee's Registration Number.
        /// </summary>
        public string? AccountNumber { get; set; }

        /// <summary>
        /// Filter on the frequencey range in MHz. Would return results equal to or higher than frequencyMin.
        /// </summary>
        public double? frequencyMin { get; set; }

        /// <summary>
        /// Filter on the frequencey range in MHz. Would return results equal to or lower than frequencyMax.
        /// </summary>
        public double? frequencyMax { get; set; }

        /// <summary>
        /// Filter based on the function of the station. The expected values are TX or RX.
        /// </summary>
        public string? StationFunction { get; set; }

        /// <summary>
        /// Filters base on the type of transmissions. The expected values are A (Analog), D (Digital).
        /// </summary>
        public char? AnalogDigital { get; set; }


        /// <summary>
        /// Filter results by location. If this is provided, only results within the specified radius 
        /// of the given latitude and longitude will be returned.
        /// </summary>
        public LocationInformation? LocationInformation { get; set; }
    }
}
