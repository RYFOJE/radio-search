using Microsoft.EntityFrameworkCore;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;

namespace Radio_Search.Importer.Canada.Data.Repositories
{
    public class TAFLDefinitionRepo : ITAFLDefinitionRepo
    {
        private CanadaImporterContext _context;

        public TAFLDefinitionRepo(
            CanadaImporterContext context) 
        { 
            _context = context;
        }

        public async Task<int> AnalogDigitalAddUpdateAsync(List<AnalogDigital> entries)
        {
            var existingItems = await _context.AnalogDigital.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.AnalogDigitalID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.AnalogDigitalID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AntennaPatternAddUpdateAsync(List<AntennaPattern> entries)
        {
            var existingItems = await _context.AntennaPatterns.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.AntennaPatternID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.AntennaPatternID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AuthorizationStatusAddUpdateAsync(List<AuthorizationStatus> entries)
        {
            var existingItems = await _context.AuthorizationStatuses.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.AuthorizationStatusID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.AuthorizationStatusID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> CommunicationTypeAddUpdateAsync(List<CommunicationType> entries)
        {
            var existingItems = await _context.CommunicationTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.CommunicationTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.CommunicationTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ConformityToFrequencyPlanAddUpdateAsync(List<ConformityFrequencyPlan> entries)
        {
            var existingItems = await _context.ConformityFrequencyPlans.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.ConformityFrequencyPlanID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.ConformityFrequencyPlanID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> CongestionZoneAddUpdateAsync(List<CongestionZoneType> entries)
        {
            var existingItems = await _context.CongestionZoneTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.CongestionZoneTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.CongestionZoneTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> FiltrationTypeAddUpdateAsync(List<FiltrationInstalledType> entries)
        {
            var existingItems = await _context.FiltrationInstalledTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.FiltrationInstalledTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.FiltrationInstalledTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ITUClassAddUpdateAsync(List<ITUClassType> entries)
        {
            var existingItems = await _context.ITUClassTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.ITUClassTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.ITUClassTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> LicenseTypeAddUpdateAsync(List<LicenseType> entries)
        {
            var existingItems = await _context.LicenseTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.LicenseTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.LicenseTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ModulationTypeAddUpdateAsync(List<ModulationType> entries)
        {
            var existingItems = await _context.ModulationTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.ModulationTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.ModulationTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> OperationalStatusAddUpdateAsync(List<OperationalStatus> entries)
        {
            var existingItems = await _context.OperationStatuses.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.OperationalStatusID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.OperationalStatusID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> PolarizationAddUpdateAsync(List<PolarizationType> entries)
        {
            var existingItems = await _context.PolarizationTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.PolarizationTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.PolarizationTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ProvincesAddUpdateAsync(List<Province> entries)
        {
            var existingItems = await _context.Provinces.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.ProvinceID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.ProvinceID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> RegulatoryServiceAddUpdateAsync(List<RegulatoryService> entries)
        {
            var existingItems = await _context.RegulatoryServices.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.RegulatoryServiceID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.RegulatoryServiceID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ServiceAddUpdateAsync(List<ServiceType> entries)
        {
            var existingItems = await _context.ServiceTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.ServiceTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.ServiceTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> StandbyTransmitterInformationAddUpdateAsync(List<StandbyTransmitterInformation> entries)
        {
            var existingItems = await _context.StandbyTransmitterInformation.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.StandbyTransmitterInformationID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.StandbyTransmitterInformationID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> StationClassAddUpdateAsync(List<StationClass> entries)
        {
            var existingItems = await _context.StationClasses.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.StationClassID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.StationClassID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> StationCostCategoryAddUpdateAsync(List<StationCostCategory> entries)
        {
            var existingItems = await _context.StationCostCategories.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.StationCostCategoryID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.StationCostCategoryID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> StationFunctionAddUpdateAsync(List<StationFunctionType> entries)
        {
            var existingItems = await _context.StationFunctionTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.StationFunctionTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.StationFunctionTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> SubserviceAddUpdateAsync(List<SubserviceType> entries)
        {
            var existingItems = await _context.SubserviceTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.SubserviceTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.SubserviceTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> TypeOfStationAddUpdateAsync(List<StationType> entries)
        {
            var existingItems = await _context.StationTypes.ToListAsync();
            var existingDict = existingItems.ToDictionary(x => x.StationTypeID);

            foreach (var item in entries)
            {
                if (existingDict.TryGetValue(item.StationTypeID, out var existing))
                {
                    existing.DescriptionEN = item.DescriptionEN;
                    existing.DescriptionFR = item.DescriptionFR;
                }
                else
                {
                    _context.Add(item);
                }
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllRowsNoTrackingAsync<T>() where T : MultiLanguageEntry
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
    }
}
