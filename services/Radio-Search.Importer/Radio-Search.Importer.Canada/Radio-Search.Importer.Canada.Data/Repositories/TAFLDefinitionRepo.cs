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

        public async Task<int> AnalogDigitalAddUpdate(List<AnalogDigital> entries)
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

        public async Task<int> AntennaPatternAddUpdate(List<AntennaPattern> entries)
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

        public async Task<int> AuthorizationStatusAddUpdate(List<AuthorizationStatus> entries)
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

        public async Task<int> CommunicationTypeAddUpdate(List<CommunicationType> entries)
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

        public async Task<int> ConformityToFrequencyPlanAddUpdate(List<ConformityFrequencyPlan> entries)
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

        public async Task<int> CongestionZoneAddUpdate(List<CongestionZoneType> entries)
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

        public async Task<int> FiltrationTypeAddUpdate(List<FiltrationInstalledType> entries)
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

        public async Task<int> ITUClassAddUpdate(List<ITUClassType> entries)
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

        public async Task<int> LicenseTypeAddUpdate(List<LicenseType> entries)
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

        public async Task<int> ModulationTypeAddUpdate(List<ModulationType> entries)
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

        public async Task<int> OperationalStatusAddUpdate(List<OperationalStatus> entries)
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

        public async Task<int> PolarizationAddUpdate(List<PolarizationType> entries)
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

        public async Task<int> ProvincesAddUpdate(List<Province> entries)
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

        public async Task<int> RegulatoryServiceAddUpdate(List<RegulatoryService> entries)
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

        public async Task<int> ServiceAddUpdate(List<ServiceType> entries)
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

        public async Task<int> StandbyTransmitterInformationAddUpdate(List<StandbyTransmitterInformation> entries)
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

        public async Task<int> StationClassAddUpdate(List<StationClass> entries)
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

        public async Task<int> StationCostCategoryAddUpdate(List<StationCostCategory> entries)
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

        public async Task<int> StationFunctionAddUpdate(List<StationFunctionType> entries)
        {
            var existingItems = await _context.stationFunctionTypes.ToListAsync();
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

        public async Task<int> SubserviceAddUpdate(List<SubserviceType> entries)
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

        public async Task<int> TypeOfStationAddUpdate(List<StationType> entries)
        {
            var existingItems = await _context.stationTypes.ToListAsync();
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
    }
}
