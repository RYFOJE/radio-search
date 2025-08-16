using NetTopologySuite.Geometries;
using Radio_Search.Importer.Canada.Data.Models.History;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class LicenseRecord : DatabaseEntry
    {
        public string CanadaLicenseRecordID { get; set; } = string.Empty;
        public int Version { get; set; } = 1;

        /// <summary>
        /// Can be Invalid due to it being removed / updated
        /// </summary>
        public bool IsValid { get; set; } = true;


        public decimal? FrequencyMHz { get; set; }
        public string? FrequencyAllocationName { get; set; }
        public string? Channel { get; set; }
        public string? InternationalCoordinationNumber { get; set; }
        public decimal? OccupiedBandwidthKHz { get; set; }
        public string? DesignationOfEmission { get; set; }
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
        public decimal? BeamwidthDeg { get; set; }
        public decimal? FrontToBackRatioDb { get; set; }
        public decimal? HeightAboveGroundM { get; set; }
        public decimal? AzimuthMainLobeDeg { get; set; }
        public decimal? VerticalElevationAngleDeg { get; set; }
        public string? StationLocation { get; set; }
        public string? StationReference { get; set; }
        public string? CallSign { get; set; }
        public int? NumberOfIdenticalStations { get; set; }
        public string? ReferenceIdentifier { get; set; }
        public Point? Location { get; set; }
        public int? GroundElevationM { get; set; }
        public decimal? AntennaStructureHeightM { get; set; }
        public string? RadiusOfOperationKm { get; set; }
        public string? SatelliteName { get; set; }
        public string? AuthorizationNumber { get; set; }
        public DateOnly? InServiceDate { get; set; }
        public string? AccountNumber { get; set; }
        public string? LicenseeName { get; set; }
        public string? LicenseeAddress { get; set; }
        public decimal? HorizontalPowerW { get; set; }
        public decimal? VerticalPowerW { get; set; }

        public StandbyTransmitterInformation? StandbyTransmitterInfo { get; set; }
        public RegulatoryService? RegulatoryService { get; set; }
        public CommunicationType? CommunicationType { get; set; }
        public ConformityFrequencyPlan? ConformityToFrequencyPlan { get; set; }
        public OperationalStatus? OperationalStatus { get; set; }
        public StationClass? StationClass { get; set; }
        public ServiceType? Service { get; set; }
        public SubserviceType? Subservice { get; set; }
        public LicenseType? LicenseType { get; set; }
        public AuthorizationStatus? AuthorizationStatus { get; set; }
        public CongestionZoneType? CongestionZone { get; set; }
        public StationType? StationType { get; set; }
        public ITUClassType? ITUClassOfStation { get; set; }
        public StationCostCategory? StationCostCategory { get; set; }
        public Province? Province { get; set; }
        public PolarizationType? Polarization { get; set; }
        public AntennaPattern? AntennaPattern { get; set; }
        public ModulationType? ModulationType { get; set; }
        public FiltrationInstalledType? FiltrationInstalledType { get; set; }
        public AnalogDigital? AnalogDigital { get; set; }
        public StationFunctionType? StationFunction { get; set; }

        // Foreign keys for relationships
        public string? StationFunctionID { get; set; }
        public short? RegulatoryServiceID { get; set; }
        public string? CommunicationTypeID { get; set; }
        public string? ConformityFrequencyPlanID { get; set; }
        public char? AnalogDigitalID { get; set; }
        public string? ModulationTypeID { get; set; }
        public char? FiltrationInstalledTypeID { get; set; }
        public string? AntennaPatternID { get; set; }
        public char? PolarizationTypeID { get; set; }
        public short? StationTypeID { get; set; }
        public string? ITUClassTypeID { get; set; }
        public short? StationCostCategoryID { get; set; }
        public string? ProvinceID { get; set; }
        public char? CongestionZoneTypeID { get; set; }
        public short? ServiceTypeID { get; set; }
        public short? SubserviceTypeID { get; set; }
        public string? LicenseTypeID { get; set; }
        public string? AuthorizationStatusID { get; set; }
        public string? OperationalStatusID { get; set; }
        public string? StationClassID { get; set; }
        public short? StandbyTransmitterInformationID { get; set; }


        public List<LicenseRecordHistory> HistoryRecords { get; set; } = new();

        public override int GetHashCode()
        {
            return (
                StationFunctionID,
                FrequencyMHz,
                ReferenceIdentifier,
                RegulatoryServiceID,
                CommunicationTypeID,
                ConformityFrequencyPlanID,
                FrequencyAllocationName,
                Channel,
                InternationalCoordinationNumber,
                AnalogDigitalID,
                OccupiedBandwidthKHz,
                DesignationOfEmission,
                ModulationTypeID,
                FiltrationInstalledTypeID,
                TxERPdBW,
                TxPowerW,
                TotalLossesDb,
                AnalogCapacityChannels,
                DigitalCapacityMbps,
                RxUnfadedReceivedSignalLevel,
                RxThresholdSignalLevel,
                AntennaManufacturer,
                AntennaModel,
                AntennaGainDbi,
                AntennaPatternID,
                BeamwidthDeg,
                FrontToBackRatioDb,
                PolarizationTypeID,
                HeightAboveGroundM,
                AzimuthMainLobeDeg,
                VerticalElevationAngleDeg,
                StationLocation,
                StationReference,
                CallSign,
                StationTypeID,
                ITUClassTypeID,
                StationCostCategoryID,
                NumberOfIdenticalStations,
                ReferenceIdentifier,
                ProvinceID,
                Location?.Y, // Latitude
                Location?.X, // Longitude
                GroundElevationM,
                AntennaStructureHeightM,
                CongestionZoneTypeID,
                RadiusOfOperationKm,
                SatelliteName,
                AuthorizationNumber,
                ServiceTypeID,
                SubserviceTypeID,
                LicenseTypeID,
                AuthorizationStatusID,
                InServiceDate,
                AccountNumber,
                LicenseeName,
                LicenseeAddress,
                OperationalStatusID,
                StationClassID,
                HorizontalPowerW,
                VerticalPowerW,
                StandbyTransmitterInformationID
            ).GetHashCode();
        }
    }
}
