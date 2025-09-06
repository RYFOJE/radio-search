using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Services.Data
{
    public class FullTAFLDefinitionRows
    {

        public List<AnalogDigital> AnalogDigitals { get; set; } = new();
        public List<AntennaPattern> AntennaPatterns { get; set; } = new();
        public List<AuthorizationStatus> AuthorizationStatuses { get; set; } = new();
        public List<CommunicationType> CommunicationTypes { get; set; } = new();
        public List<ConformityFrequencyPlan> ConformityFrequencyPlans { get; set; } = new();
        public List<CongestionZoneType> CongestionZoneTypes { get; set; } = new();
        public List<FiltrationInstalledType> FiltrationInstalledTypes { get; set;} = new();
        public List<ITUClassType> ITUClassTypes { get; set; } = new();
        public List<LicenseType> LicenseTypes { get; set; } = new();
        public List<ModulationType> ModulationTypes { get; set; } = new();
        public List<OperationalStatus> OperationalStatuses { get; set; } = new();
        public List<PolarizationType> PolarizationTypes { get; set;} = new();
        public List<Province> Provinces { get; set; } = new();
        public List<RegulatoryService> RegulatoryServices { get; set; } = new();
        public List<ServiceType> ServiceTypes { get; set; } = new();
        public List<StandbyTransmitterInformation> StandingTransmitters { get; set; } = new();
        public List<StationClass> StationClasses { get; set; } = new();
        public List<StationCostCategory> StationCostCategories { get; set;} = new();
        public List<StationFunctionType> StationFunctionTypes { get; set; } = new();
        public List<StationType> StationTypes { get; set; } = new();
        public List<SubserviceType> SubserviceTypes { get; set; } = new();

    }
}
