using NetTopologySuite.Geometries;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class LicenseRecord : DatabaseEntry
    {
        public string? LicenseRecordID { get; set; }

        /// <summary>
        /// Can be Invalid due to it being removed / updated
        /// </summary>
        public bool IsValid { get; set; } = true;
        public StationFunctionType? StationFunction { get; set; }
        public decimal? FrequencyMHz { get; set; }
        public RegulatoryService? RegulatoryService { get; set; }
        public CommunicationType? CommunicationType { get; set; }
        public ConformityFrequencyPlan? ConformityToFrequencyPlan { get; set; }
        public string? FrequencyAllocationName { get; set; }
        public string? Channel { get; set; }
        public string? InternationalCoordinationNumber { get; set; }
        public AnalogDigital? AnalogDigital { get; set; }
        public decimal? OccupiedBandwidthKHz { get; set; }
        public string? DesignationOfEmission { get; set; }
        public ModulationType? ModulationType { get; set; }
        public FiltrationInstalledType? FiltrationInstalled { get; set; }
        public decimal? TxERPdBW { get; set; }
        public decimal? TxPowerW { get; set; }
        public decimal? TotalLossesDb { get; set; }
        public decimal? AnalogCapacityChannels { get; set; }
        public decimal? DigitalCapacityMbps { get; set; }
        public string? RxUnfadedReceivedSignalLevel { get; set; }
        public string? RxThresholdSignalLevel { get; set; }
        public string? AntennaManufacturer { get; set; }
        public string? AntennaModel { get; set; }
        public decimal? AntennaGainDbi { get; set; }
        public AntennaPattern? AntennaPattern { get; set; }
        public decimal? BeamwidthDeg { get; set; }
        public decimal? FrontToBackRatioDb { get; set; }
        public PolarizationType? Polarization { get; set; }
        public decimal? HeightAboveGroundM { get; set; }
        public decimal? AzimuthMainLobeDeg { get; set; }
        public decimal? VerticalElevationAngleDeg { get; set; }
        public string? StationLocation { get; set; }
        public string? StationReference { get; set; }
        public string? CallSign { get; set; }
        public StationType? StationType { get; set; }
        public ITUClassType? ITUClassOfStation { get; set; }
        public StationCostCategory? StationCostCategory { get; set; }
        public int? NumberOfIdenticalStations { get; set; }
        public string? ReferenceIdentifier { get; set; }
        public Province? Province { get; set; }
        public Point? Location { get; set; }
        public int? GroundElevationM { get; set; }
        public decimal? AntennaStructureHeightM { get; set; }
        public CongestionZoneType? CongestionZone { get; set; }
        public string? RadiusOfOperationKm { get; set; }
        public string? SatelliteName { get; set; }
        public string? AuthorizationNumber { get; set; }
        public ServiceType? Service { get; set; }
        public SubserviceType? Subservice { get; set; }
        public LicenseType? LicenceType { get; set; }
        public AuthorizationStatus? AuthorizationStatus { get; set; }
        public string? InServiceDate { get; set; }
        public string? AccountNumber { get; set; }
        public string? LicenseeName { get; set; }
        public string? LicenseeAddress { get; set; }
        public OperationalStatus? OperationalStatus { get; set; }
        public StationClass? StationClass { get; set; }
        public decimal? HorizontalPowerW { get; set; }
        public decimal? VerticalPowerW { get; set; }
        public StandbyTransmitterInformation? StandbyTransmitterInfo { get; set; }

        public List<LicenseRecordHistory>? History { get; set; }
    }
}
