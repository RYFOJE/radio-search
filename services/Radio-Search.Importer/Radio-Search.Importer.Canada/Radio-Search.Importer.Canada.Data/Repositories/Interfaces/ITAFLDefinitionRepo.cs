using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Data.Repositories.Interfaces
{
    public interface ITAFLDefinitionRepo
    {
        Task<int> StationFunctionAddUpdate(List<StationFunctionType> entries);
        Task<int> RegulatoryServiceAddUpdate(List<RegulatoryService> entries);
        Task<int> CommunicationTypeAddUpdate(List<CommunicationType> entries);
        Task<int> ConformityToFrequencyPlanAddUpdate(List<ConformityFrequencyPlan> entries);
        Task<int> AnalogDigitalAddUpdate(List<AnalogDigital> entries);
        Task<int> ModulationTypeAddUpdate(List<ModulationType> entries);
        Task<int> FiltrationTypeAddUpdate(List<FiltrationInstalledType> entries);
        Task<int> AntennaPatternAddUpdate(List<AntennaPattern> entries);
        Task<int> PolarizationAddUpdate(List<PolarizationType> entries);
        Task<int> TypeOfStationAddUpdate(List<StationType> entries);
        Task<int> ITUClassAddUpdate(List<ITUClassType> entries);
        Task<int> StationCostCategoryAddUpdate(List<StationCostCategory> entries);
        Task<int> ProvincesAddUpdate(List<Province> entries);
        Task<int> CongestionZoneAddUpdate(List<CongestionZoneType> entries);
        Task<int> ServiceAddUpdate(List<ServiceType> entries);
        Task<int> SubserviceAddUpdate(List<SubserviceType> entries);
        Task<int> LicenseTypeAddUpdate(List<LicenseType> entries);
        Task<int> AuthorizationStatusAddUpdate(List<AuthorizationStatus> entries);
        Task<int> OperationalStatusAddUpdate(List<OperationalStatus> entries);
        Task<int> StationClassAddUpdate(List<StationClass> entries);
        Task<int> StandbyTransmitterInformationAddUpdate(List<StandbyTransmitterInformation> entries);
    }
}
