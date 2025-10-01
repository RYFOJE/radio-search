using System.Drawing;

namespace Radio_Search.Querying.Canada.Data_Contracts.V1.Responses
{
    public class LicenseDataContract
    {
        /// <summary>Unique Canada license record identifier.</summary>
        public int CanadaLicenseRecordID { get; set; }

        /// <summary>Frequency in MHz.</summary>
        public decimal? FrequencyMHz { get; set; }
        
        /// <summary>Name of frequency allocation.</summary>
        public string? FrequencyAllocationName { get; set; }
        
        /// <summary>Channel identifier.</summary>
        public string? Channel { get; set; }
        
        /// <summary>International coordination number.</summary>
        public string? InternationalCoordinationNumber { get; set; }
        
        /// <summary>Occupied bandwidth in kHz.</summary>
        public decimal? OccupiedBandwidthKHz { get; set; }
        
        /// <summary>Designation of emission.</summary>
        public string? DesignationOfEmission { get; set; }
        
        /// <summary>Transmitter ERP in dBW.</summary>
        public decimal? TxERPdBW { get; set; }
        
        /// <summary>Transmitter power in watts.</summary>
        public decimal? TxPowerW { get; set; }
        
        /// <summary>Total losses in dB.</summary>
        public decimal? TotalLossesDb { get; set; }
        
        /// <summary>Analog capacity in channels.</summary>
        public decimal? AnalogCapacityChannels { get; set; }
        
        /// <summary>Digital capacity in Mbps.</summary>
        public decimal? DigitalCapacityMbps { get; set; }
        
        /// <summary>Unfaded received signal level.</summary>
        public string? RxUnfadedReceivedSignalLevel { get; set; }
        
        /// <summary>Threshold signal level for receiver.</summary>
        public string? RxThresholdSignalLevel { get; set; }
        
        /// <summary>Antenna manufacturer name.</summary>
        public string? AntennaManufacturer { get; set; }
        
        /// <summary>Antenna model name.</summary>
        public string? AntennaModel { get; set; }
        
        /// <summary>Antenna gain in dBi.</summary>
        public decimal? AntennaGainDbi { get; set; }
        
        /// <summary>Beamwidth in degrees.</summary>
        public decimal? BeamwidthDeg { get; set; }
        
        /// <summary>Front-to-back ratio in dB.</summary>
        public decimal? FrontToBackRatioDb { get; set; }
        
        /// <summary>Height above ground in meters.</summary>
        public decimal? HeightAboveGroundM { get; set; }
        
        /// <summary>Main lobe azimuth in degrees.</summary>
        public decimal? AzimuthMainLobeDeg { get; set; }
        
        /// <summary>Vertical elevation angle in degrees.</summary>
        public decimal? VerticalElevationAngleDeg { get; set; }
        
        /// <summary>Station location description.</summary>
        public string? StationLocation { get; set; }
        
        /// <summary>Station reference identifier.</summary>
        public string? StationReference { get; set; }
        
        /// <summary>Station call sign.</summary>
        public string? CallSign { get; set; }
        
        /// <summary>Number of identical stations.</summary>
        public int? NumberOfIdenticalStations { get; set; }
        
        /// <summary>Reference identifier.</summary>
        public string? ReferenceIdentifier { get; set; }
        
        /// <summary>Geographic location as a point.</summary>
        public Point? Location { get; set; }
        
        /// <summary>Ground elevation in meters.</summary>
        public int? GroundElevationM { get; set; }
        
        /// <summary>Antenna structure height in meters.</summary>
        public decimal? AntennaStructureHeightM { get; set; }
        
        /// <summary>Radius of operation in km.</summary>
        public string? RadiusOfOperationKm { get; set; }
        
        /// <summary>Satellite name.</summary>
        public string? SatelliteName { get; set; }
        
        /// <summary>Authorization number.</summary>
        public string? AuthorizationNumber { get; set; }
        
        /// <summary>Date when service started.</summary>
        public DateOnly? InServiceDate { get; set; }
        
        /// <summary>Account number.</summary>
        public string? AccountNumber { get; set; }
        
        /// <summary>Licensee name.</summary>
        public string? LicenseeName { get; set; }
        
        /// <summary>Licensee address.</summary>
        public string? LicenseeAddress { get; set; }
        
        /// <summary>Horizontal power in watts.</summary>
        public decimal? HorizontalPowerW { get; set; }
        
        /// <summary>Vertical power in watts.</summary>
        public decimal? VerticalPowerW { get; set; }
        
        /// <summary>Station function identifier.</summary>
        public string? StationFunctionID { get; set; }
        
        /// <summary>Regulatory service identifier.</summary>
        public short? RegulatoryServiceID { get; set; }
        
        /// <summary>Communication type identifier.</summary>
        public string? CommunicationTypeID { get; set; }
        
        /// <summary>Conformity frequency plan identifier.</summary>
        public string? ConformityFrequencyPlanID { get; set; }
        
        /// <summary>Analog/digital identifier.</summary>
        public char? AnalogDigitalID { get; set; }
        
        /// <summary>Modulation type identifier.</summary>
        public string? ModulationTypeID { get; set; }
        
        /// <summary>Filtration installed type identifier.</summary>
        public char? FiltrationInstalledTypeID { get; set; }
        
        /// <summary>Antenna pattern identifier.</summary>
        public string? AntennaPatternID { get; set; }
        
        /// <summary>Polarization type identifier.</summary>
        public char? PolarizationTypeID { get; set; }
        
        /// <summary>Station type identifier.</summary>
        public short? StationTypeID { get; set; }
        
        /// <summary>ITU class type identifier.</summary>
        public string? ITUClassTypeID { get; set; }
        
        /// <summary>Station cost category identifier.</summary>
        public short? StationCostCategoryID { get; set; }
        
        /// <summary>Province identifier.</summary>
        public string? ProvinceID { get; set; }
        
        /// <summary>Congestion zone type identifier.</summary>
        public char? CongestionZoneTypeID { get; set; }
        
        /// <summary>Service type identifier.</summary>
        public short? ServiceTypeID { get; set; }
        
        /// <summary>Subservice type identifier.</summary>
        public short? SubserviceTypeID { get; set; }
        
        /// <summary>License type identifier.</summary>
        public string? LicenseTypeID { get; set; }
        
        /// <summary>Authorization status identifier.</summary>
        public string? AuthorizationStatusID { get; set; }
        
        /// <summary>Operational status identifier.</summary>
        public string? OperationalStatusID { get; set; }
        
        /// <summary>Station class identifier.</summary>
        public string? StationClassID { get; set; }
        
        /// <summary>Standby transmitter information identifier.</summary>
        public short? StandbyTransmitterInformationID { get; set; }
    }
}
