using AutoMapper;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Mappings
{
    public class TAFLDefinitionProfile : Profile
    {

        public TAFLDefinitionProfile()
        {
            // StationFunction
            CreateMap<TableDefinitionRow, StationFunctionType>()
                .ForMember(dest => dest.StationFunctionTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<StationFunctionType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationFunctionTypeID));


            // RegulatoryService
            CreateMap<TableDefinitionRow, RegulatoryService>()
                .ForMember(
                    dest => dest.RegulatoryServiceID, opt => 
                opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<RegulatoryService, TableDefinitionRow>()
                .ForMember(
                    dest => dest.Code, 
                opt => opt.MapFrom(src => src.RegulatoryServiceID.ToString()));


            // CommunicationType
            CreateMap<TableDefinitionRow, CommunicationType>()
                .ForMember(dest => dest.CommunicationTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<CommunicationType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CommunicationTypeID));


            // ConformityToFrequencyPlan
            CreateMap<TableDefinitionRow, ConformityFrequencyPlan>()
                .ForMember(
                    dest => dest.ConformityFrequencyPlanID, 
                    opt => opt.MapFrom(src => src.Code[0]
                ));

            CreateMap<ConformityFrequencyPlan, TableDefinitionRow>()
                .ForMember(
                    dest => dest.Code, 
                opt => opt.MapFrom(src => src.ConformityFrequencyPlanID.ToString()));


            // AnalogDigital
            CreateMap<TableDefinitionRow, AnalogDigital>()
                .ForMember(dest => dest.AnalogDigitalID, opt => opt.MapFrom(
                    src => src.Code[0]));

            CreateMap<AnalogDigital, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(
                    src => src.AnalogDigitalID.ToString()));


            // ModulationType
            CreateMap<TableDefinitionRow, ModulationType>()
                .ForMember(dest => dest.ModulationTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<ModulationType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ModulationTypeID));


            // FiltrationType
            CreateMap<TableDefinitionRow, FiltrationInstalledType>()
                .ForMember(dest => dest.FiltrationInstalledTypeID, opt => opt.MapFrom(src => src.Code[0]));

            CreateMap<FiltrationInstalledType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FiltrationInstalledTypeID.ToString()));


            // AntennaPattern
            CreateMap<TableDefinitionRow, AntennaPattern>()
                .ForMember(dest => dest.AntennaPatternID, opt => opt.MapFrom(src => src.Code));

            CreateMap<AntennaPattern, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AntennaPatternID));


            // Polarization
            CreateMap<TableDefinitionRow, PolarizationType>()
                .ForMember(dest => dest.PolarizationTypeID, opt => opt.MapFrom(src => src.Code[0]));

            CreateMap<PolarizationType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PolarizationTypeID.ToString()));


            // TypeOfStation
            CreateMap<TableDefinitionRow, StationType>()
                .ForMember(dest => dest.StationTypeID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<StationType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationTypeID.ToString()));


            // ITUClass
            CreateMap<TableDefinitionRow, ITUClassType>()
                .ForMember(dest => dest.ITUClassTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<ITUClassType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ITUClassTypeID));


            // StationCostCategory
            CreateMap<TableDefinitionRow, StationCostCategory>()
                .ForMember(dest => dest.StationCostCategoryID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<StationCostCategory, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationCostCategoryID.ToString()));


            // Provinces
            CreateMap<TableDefinitionRow, Province>()
                .ForMember(dest => dest.ProvinceID, opt => opt.MapFrom(src => src.Code));

            CreateMap<Province, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ProvinceID));


            // CongestionZone
            CreateMap<TableDefinitionRow, CongestionZoneType>()
                .ForMember(dest => dest.CongestionZoneTypeID, opt => opt.MapFrom(src => src.Code[0]));

            CreateMap<CongestionZoneType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CongestionZoneTypeID.ToString()));


            // Service
            CreateMap<TableDefinitionRow, ServiceType>()
                .ForMember(dest => dest.ServiceTypeID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<ServiceType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ServiceTypeID.ToString()));


            // Subservice
            CreateMap<TableDefinitionRow, SubserviceType>()
                .ForMember(dest => dest.SubserviceTypeID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<SubserviceType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SubserviceTypeID.ToString()));


            // LicenseType
            CreateMap<TableDefinitionRow, LicenseType>()
                .ForMember(dest => dest.LicenseTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<LicenseType, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.LicenseTypeID));


            // AuthorizationStatus
            CreateMap<TableDefinitionRow, AuthorizationStatus>()
                .ForMember(dest => dest.AuthorizationStatusID, opt => opt.MapFrom(src => src.Code));

            CreateMap<AuthorizationStatus, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AuthorizationStatusID));


            // OperationalStatus
            CreateMap<TableDefinitionRow, OperationalStatus>()
                .ForMember(dest => dest.OperationalStatusID, opt => opt.MapFrom(src => src.Code));

            CreateMap<OperationalStatus, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.OperationalStatusID));


            // StationClass
            CreateMap<TableDefinitionRow, StationClass>()
                .ForMember(dest => dest.StationClassID, opt => opt.MapFrom(src => src.Code));

            CreateMap<StationClass, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationClassID));


            // StationClass
            CreateMap<TableDefinitionRow, StandbyTransmitterInformation>()
                .ForMember(dest => dest.StandbyTransmitterInformationID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<StandbyTransmitterInformation, TableDefinitionRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StandbyTransmitterInformationID.ToString()));
        }

    }
}
