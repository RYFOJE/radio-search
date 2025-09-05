using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Data.Repositories.Interfaces
{
    public interface ITAFLDefinitionRepo
    {
        Task<int> StationFunctionAddUpdateAsync(List<StationFunctionType> entries);
        Task<int> RegulatoryServiceAddUpdateAsync(List<RegulatoryService> entries);
        Task<int> CommunicationTypeAddUpdateAsync(List<CommunicationType> entries);
        Task<int> ConformityToFrequencyPlanAddUpdateAsync(List<ConformityFrequencyPlan> entries);
        Task<int> AnalogDigitalAddUpdateAsync(List<AnalogDigital> entries);
        Task<int> ModulationTypeAddUpdateAsync(List<ModulationType> entries);
        Task<int> FiltrationTypeAddUpdateAsync(List<FiltrationInstalledType> entries);
        Task<int> AntennaPatternAddUpdateAsync(List<AntennaPattern> entries);
        Task<int> PolarizationAddUpdateAsync(List<PolarizationType> entries);
        Task<int> TypeOfStationAddUpdateAsync(List<StationType> entries);
        Task<int> ITUClassAddUpdateAsync(List<ITUClassType> entries);
        Task<int> StationCostCategoryAddUpdateAsync(List<StationCostCategory> entries);
        Task<int> ProvincesAddUpdateAsync(List<Province> entries);
        Task<int> CongestionZoneAddUpdateAsync(List<CongestionZoneType> entries);
        Task<int> ServiceAddUpdateAsync(List<ServiceType> entries);
        Task<int> SubserviceAddUpdateAsync(List<SubserviceType> entries);
        Task<int> LicenseTypeAddUpdateAsync(List<LicenseType> entries);
        Task<int> AuthorizationStatusAddUpdateAsync(List<AuthorizationStatus> entries);
        Task<int> OperationalStatusAddUpdateAsync(List<OperationalStatus> entries);
        Task<int> StationClassAddUpdateAsync(List<StationClass> entries);
        Task<int> StandbyTransmitterInformationAddUpdateAsync(List<StandbyTransmitterInformation> entries);
        Task<List<T>> GetAllRowsNoTrackingAsync<T>() where T : MultiLanguageEntry;
    }
}
