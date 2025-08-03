using CsvHelper.Configuration.Attributes;
using Radio_Search.Importer.Canada.Data.Models.License;
using System;

namespace Radio_Search.Importer.Canada.Services.Data
{
    public class TAFLEntryRawRow
    {
        [Index(0)] public string? StationFunctionID { get; set; }
        [Index(1)] public decimal? FrequencyMHz { get; set; }
        [Index(2)] public string LicenseRecordID { get; set; } = String.Empty;
        [Index(3)] public string? RegulatoryServiceID { get; set; }
        [Index(4)] public string? CommunicationTypeID { get; set; }
        [Index(5)] public string? ConformityFrequencyPlanID { get; set; }
        [Index(6)] public string? FrequencyAllocationName { get; set; }
        [Index(7)] public string? Channel { get; set; }
        [Index(8)] public string? InternationalCoordinationNumber { get; set; }
        [Index(9)] public string? AnalogDigitalID { get; set; }
        [Index(10)] public decimal? OccupiedBandwidthKHz { get; set; }
        [Index(11)] public string? DesignationOfEmission { get; set; }
        [Index(12)] public string? ModulationTypeID { get; set; }
        [Index(13)] public string? FiltrationInstalledTypeID { get; set; }
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
        [Index(24)] public string? AntennaPatternID { get; set; }
        [Index(25)] public decimal? BeamwidthDeg { get; set; }
        [Index(26)] public decimal? FrontToBackRatioDb { get; set; }
        [Index(27)] public string? PolarizationTypeID { get; set; }
        [Index(28)] public decimal? HeightAboveGroundM { get; set; }
        [Index(29)] public decimal? AzimuthMainLobeDeg { get; set; }
        [Index(30)] public decimal? VerticalElevationAngleDeg { get; set; }
        [Index(31)] public string? StationLocation { get; set; }
        [Index(32)] public string? StationReference { get; set; }
        [Index(33)] public string? CallSign { get; set; }
        [Index(34)] public string? StationTypeID { get; set; }
        [Index(35)] public string? ITUClassTypeID { get; set; }
        [Index(36)] public int? StationCostCategoryID { get; set; }
        [Index(37)] public int? NumberOfIdenticalStations { get; set; }
        [Index(38)] public string? ReferenceIdentifier { get; set; }
        [Index(39)] public string? ProvinceID { get; set; }
        [Index(40)] public decimal? Latitude { get; set; }
        [Index(41)] public decimal? Longitude { get; set; }
        [Index(42)] public int? GroundElevationM { get; set; }
        [Index(43)] public decimal? AntennaStructureHeightM { get; set; }
        [Index(44)] public string? CongestionZoneTypeID { get; set; }
        [Index(45)] public string? RadiusOfOperationKm { get; set; }
        [Index(46)] public string? SatelliteName { get; set; }
        [Index(47)] public string? AuthorizationNumber { get; set; }
        [Index(48)] public int? ServiceTypeID { get; set; }
        [Index(49)] public int? SubserviceTypeID { get; set; }
        [Index(50)] public string? LicenseTypeID { get; set; }
        [Index(51)] public string? AuthorizationStatusID { get; set; }
        [Index(52)] public DateOnly? InServiceDate { get; set; }
        [Index(53)] public string? AccountNumber { get; set; }
        [Index(54)] public string? LicenseeName { get; set; }
        [Index(55)] public string? LicenseeAddress { get; set; }
        [Index(56)] public string? OperationalStatusID { get; set; }
        [Index(57)] public string? StationClassID { get; set; }
        [Index(58)] public decimal? HorizontalPowerW { get; set; }
        [Index(59)] public decimal? VerticalPowerW { get; set; }
        [Index(60)] public int? StandbyTransmitterInformationID { get; set; }

        public bool Equals(LicenseRecord? other)
        {
            if (other is null) return false;

            // Compare each field, converting as needed
            bool areEqual =
                string.Equals(StationFunctionID, other.StationFunctionID, StringComparison.OrdinalIgnoreCase) &&
                FrequencyMHz == other.FrequencyMHz &&
                string.Equals(RegulatoryServiceID, other.RegulatoryServiceID?.ToString(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(CommunicationTypeID, other.CommunicationTypeID, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(ConformityFrequencyPlanID, other.ConformityFrequencyPlanID, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(FrequencyAllocationName, other.FrequencyAllocationName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Channel, other.Channel, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(InternationalCoordinationNumber, other.InternationalCoordinationNumber, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(AnalogDigitalID, other.AnalogDigitalID?.ToString(), StringComparison.OrdinalIgnoreCase) &&
                OccupiedBandwidthKHz == other.OccupiedBandwidthKHz &&
                string.Equals(DesignationOfEmission, other.DesignationOfEmission, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(ModulationTypeID, other.ModulationTypeID, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(FiltrationInstalledTypeID, other.FiltrationInstalledTypeID?.ToString(), StringComparison.OrdinalIgnoreCase) &&
                TxERPdBW == other.TxERPdBW &&
                TxPowerW == other.TxPowerW &&
                TotalLossesDb == other.TotalLossesDb &&
                AnalogCapacityChannels == other.AnalogCapacityChannels &&
                DigitalCapacityMbps == other.DigitalCapacityMbps &&
                string.Equals(RxUnfadedReceivedSignalLevel, other.RxUnfadedReceivedSignalLevel, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(RxThresholdSignalLevel, other.RxThresholdSignalLevel, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(AntennaManufacturer, other.AntennaManufacturer, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(AntennaModel, other.AntennaModel, StringComparison.OrdinalIgnoreCase) &&
                AntennaGainDbi == other.AntennaGainDbi &&
                string.Equals(AntennaPatternID, other.AntennaPatternID, StringComparison.OrdinalIgnoreCase) &&
                BeamwidthDeg == other.BeamwidthDeg &&
                FrontToBackRatioDb == other.FrontToBackRatioDb &&
                string.Equals(PolarizationTypeID, other.PolarizationTypeID?.ToString(), StringComparison.OrdinalIgnoreCase) &&
                HeightAboveGroundM == other.HeightAboveGroundM &&
                AzimuthMainLobeDeg == other.AzimuthMainLobeDeg &&
                VerticalElevationAngleDeg == other.VerticalElevationAngleDeg &&
                string.Equals(StationLocation, other.StationLocation, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(StationReference, other.StationReference, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(CallSign, other.CallSign, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(StationTypeID, other.StationTypeID?.ToString(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(ITUClassTypeID, other.ITUClassTypeID, StringComparison.OrdinalIgnoreCase) &&
                (StationCostCategoryID == (other.StationCostCategoryID.HasValue ? (int?)other.StationCostCategoryID.Value : null)) &&
                NumberOfIdenticalStations == other.NumberOfIdenticalStations &&
                string.Equals(ReferenceIdentifier, other.ReferenceIdentifier, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(ProvinceID, other.ProvinceID, StringComparison.OrdinalIgnoreCase) &&
                // Location: compare Latitude/Longitude to Point.Y/X
                Nullable.Equals(Latitude, other.Location?.Y) &&
                Nullable.Equals(Longitude, other.Location?.X) &&
                GroundElevationM == other.GroundElevationM &&
                AntennaStructureHeightM == other.AntennaStructureHeightM &&
                string.Equals(CongestionZoneTypeID, other.CongestionZoneTypeID?.ToString(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(RadiusOfOperationKm, other.RadiusOfOperationKm, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(SatelliteName, other.SatelliteName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(AuthorizationNumber, other.AuthorizationNumber, StringComparison.OrdinalIgnoreCase) &&
                (ServiceTypeID == (other.ServiceTypeID.HasValue ? (int?)other.ServiceTypeID.Value : null)) &&
                (SubserviceTypeID == (other.SubserviceTypeID.HasValue ? (int?)other.SubserviceTypeID.Value : null)) &&
                string.Equals(LicenseTypeID, other.LicenseTypeID, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(AuthorizationStatusID, other.AuthorizationStatusID, StringComparison.OrdinalIgnoreCase) &&
                DateOnly.Equals(InServiceDate, other.InServiceDate) &&
                string.Equals(AccountNumber, other.AccountNumber, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(LicenseeName, other.LicenseeName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(LicenseeAddress, other.LicenseeAddress, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(OperationalStatusID, other.OperationalStatusID, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(StationClassID, other.StationClassID, StringComparison.OrdinalIgnoreCase) &&
                HorizontalPowerW == other.HorizontalPowerW &&
                VerticalPowerW == other.VerticalPowerW &&
                (StandbyTransmitterInformationID == (other.StandbyTransmitterInformationID.HasValue ? (int?)other.StandbyTransmitterInformationID.Value : null));

            return areEqual;
        }

        public static bool operator ==(TAFLEntryRawRow left, LicenseRecord right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(TAFLEntryRawRow left, LicenseRecord right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is LicenseRecord record)
                return Equals(record);
            return base.Equals(obj);
        }
    }
}
