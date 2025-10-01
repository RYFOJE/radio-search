namespace Radio_Search.Querying.Generic.Data_Contracts.V1
{
    public class LocationInformation
    { 
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public required int RadiusInMeters { get; set; }
    }
}
