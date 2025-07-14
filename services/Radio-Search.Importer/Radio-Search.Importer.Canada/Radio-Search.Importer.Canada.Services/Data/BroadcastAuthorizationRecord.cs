using CsvHelper.Configuration.Attributes;

namespace Radio_Search.Importer.Canada.Services.Data
{
    public class BroadcastAuthorizationRecord
    {
        [Index(0)] public string? StationFunction { get; set; }
        [Index(1)] public decimal? FrequencyMHz { get; set; }
        [Index(2)] public string? FrequencyRecordIdentifier { get; set; }
        [Index(3)] public string? RegulatoryService { get; set; }
        [Index(4)] public string? CommunicationType { get; set; }
        [Index(5)] public string? ConformityToFrequencyPlan { get; set; }
        [Index(6)] public string? FrequencyAllocationName { get; set; }
        [Index(7)] public string? Channel { get; set; }
        [Index(8)] public string? InternationalCoordinationNumber { get; set; }
        [Index(9)] public string? AnalogDigital { get; set; }
        [Index(10)] public decimal? OccupiedBandwidthKHz { get; set; }
        [Index(11)] public string? DesignationOfEmission { get; set; }
        [Index(12)] public string? ModulationType { get; set; }
        [Index(13)] public string? FiltrationInstalled { get; set; }
        [Index(14)] public decimal? TxERPdBW { get; set; }
        [Index(15)] public decimal? TxPowerW { get; set; }
        [Index(16)] public decimal? TotalLossesDb { get; set; }
        [Index(17)] public decimal? AnalogCapacityChannels { get; set; }
        [Index(18)] public decimal? DigitalCapacityMbps { get; set; }
        [Index(19)] public string? RxUnfadedReceivedSignalLevel { get; set; }
        [Index(20)] public string? RxThresholdSignalLevel { get; set; }
        [Index(21)] public string? AntennaManufacturer { get; set; }
        [Index(22)] public string? AntennaModel { get; set; }
        [Index(23)] public decimal? AntennaGainDbi { get; set; }
        [Index(24)] public string? AntennaPattern { get; set; }
        [Index(25)] public decimal? BeamwidthDeg { get; set; }
        [Index(26)] public decimal? FrontToBackRatioDb { get; set; }
        [Index(27)] public string? Polarization { get; set; }
        [Index(28)] public decimal? HeightAboveGroundM { get; set; }
        [Index(29)] public decimal? AzimuthMainLobeDeg { get; set; }
        [Index(30)] public decimal? VerticalElevationAngleDeg { get; set; }
        [Index(31)] public string? StationLocation { get; set; }
        [Index(32)] public string? StationReference { get; set; }
        [Index(33)] public string? CallSign { get; set; }
        [Index(34)] public string? StationType { get; set; }
        [Index(35)] public string? ITUClassOfStation { get; set; }
        [Index(36)] public int? StationCostCategory { get; set; }
        [Index(37)] public int? NumberOfIdenticalStations { get; set; }
        [Index(38)] public string? ReferenceIdentifier { get; set; }
        [Index(39)] public string? Province { get; set; }
        [Index(40)] public decimal? Latitude { get; set; }
        [Index(41)] public decimal? Longitude { get; set; }
        [Index(42)] public int? GroundElevationM { get; set; }
        [Index(43)] public decimal? AntennaStructureHeightM { get; set; }
        [Index(44)] public string? CongestionZone { get; set; }
        [Index(45)] public string? RadiusOfOperationKm { get; set; }
        [Index(46)] public string? SatelliteName { get; set; }
        [Index(47)] public string? AuthorizationNumber { get; set; }
        [Index(48)] public int? Service { get; set; }
        [Index(49)] public int? Subservice { get; set; }
        [Index(50)] public string? LicenceType { get; set; }
        [Index(51)] public string? AuthorizationStatus { get; set; }
        [Index(52)] public string? InServiceDate { get; set; }
        [Index(53)] public string? AccountNumber { get; set; }
        [Index(54)] public string? LicenseeName { get; set; }
        [Index(55)] public string? LicenseeAddress { get; set; }
        [Index(56)] public string? OperationalStatus { get; set; }
        [Index(57)] public string? StationClass { get; set; }
        [Index(58)] public decimal? HorizontalPowerW { get; set; }
        [Index(59)] public decimal? VerticalPowerW { get; set; }
        [Index(60)] public int? StandbyTransmitterInfo { get; set; }
    }
}
