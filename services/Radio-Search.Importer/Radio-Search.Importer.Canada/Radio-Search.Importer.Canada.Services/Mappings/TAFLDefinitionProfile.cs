using AutoMapper;
using Radio_Search.Canada.Models.License;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Mappings
{
    public class TAFLDefinitionProfile : Profile
    {

        public TAFLDefinitionProfile()
        {
            // StationFunction
            CreateMap<TAFLDefinitionRawRow, StationFunctionType>()
                .ForMember(dest => dest.StationFunctionTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<StationFunctionType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationFunctionTypeID));


            // RegulatoryService
            CreateMap<TAFLDefinitionRawRow, RegulatoryService>()
                .ForMember(
                    dest => dest.RegulatoryServiceID, opt => 
                opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<RegulatoryService, TAFLDefinitionRawRow>()
                .ForMember(
                    dest => dest.Code, 
                opt => opt.MapFrom(src => src.RegulatoryServiceID.ToString()));


            // CommunicationType
            CreateMap<TAFLDefinitionRawRow, CommunicationType>()
                .ForMember(dest => dest.CommunicationTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<CommunicationType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CommunicationTypeID));


            // ConformityToFrequencyPlan
            CreateMap<TAFLDefinitionRawRow, ConformityFrequencyPlan>()
                .ForMember(
                    dest => dest.ConformityFrequencyPlanID, 
                    opt => opt.MapFrom(src => src.Code[0]
                ));

            CreateMap<ConformityFrequencyPlan, TAFLDefinitionRawRow>()
                .ForMember(
                    dest => dest.Code, 
                opt => opt.MapFrom(src => src.ConformityFrequencyPlanID.ToString()));


            // AnalogDigital
            CreateMap<TAFLDefinitionRawRow, AnalogDigital>()
                .ForMember(dest => dest.AnalogDigitalID, opt => opt.MapFrom(
                    src => src.Code[0]));

            CreateMap<AnalogDigital, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(
                    src => src.AnalogDigitalID.ToString()));


            // ModulationType
            CreateMap<TAFLDefinitionRawRow, ModulationType>()
                .ForMember(dest => dest.ModulationTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<ModulationType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ModulationTypeID));


            // FiltrationType
            CreateMap<TAFLDefinitionRawRow, FiltrationInstalledType>()
                .ForMember(dest => dest.FiltrationInstalledTypeID, opt => opt.MapFrom(src => src.Code[0]));

            CreateMap<FiltrationInstalledType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.FiltrationInstalledTypeID.ToString()));


            // AntennaPattern
            CreateMap<TAFLDefinitionRawRow, AntennaPattern>()
                .ForMember(dest => dest.AntennaPatternID, opt => opt.MapFrom(src => src.Code));

            CreateMap<AntennaPattern, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AntennaPatternID));


            // Polarization
            CreateMap<TAFLDefinitionRawRow, PolarizationType>()
                .ForMember(dest => dest.PolarizationTypeID, opt => opt.MapFrom(src => src.Code[0]));

            CreateMap<PolarizationType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PolarizationTypeID.ToString()));


            // TypeOfStation
            CreateMap<TAFLDefinitionRawRow, StationType>()
                .ForMember(dest => dest.StationTypeID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<StationType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationTypeID.ToString()));


            // ITUClass
            CreateMap<TAFLDefinitionRawRow, ITUClassType>()
                .ForMember(dest => dest.ITUClassTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<ITUClassType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ITUClassTypeID));


            // StationCostCategory
            CreateMap<TAFLDefinitionRawRow, StationCostCategory>()
                .ForMember(dest => dest.StationCostCategoryID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<StationCostCategory, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationCostCategoryID.ToString()));


            // Provinces
            CreateMap<TAFLDefinitionRawRow, Province>()
                .ForMember(dest => dest.ProvinceID, opt => opt.MapFrom(src => src.Code));

            CreateMap<Province, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ProvinceID));


            // CongestionZone
            CreateMap<TAFLDefinitionRawRow, CongestionZoneType>()
                .ForMember(dest => dest.CongestionZoneTypeID, opt => opt.MapFrom(src => src.Code[0]));

            CreateMap<CongestionZoneType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CongestionZoneTypeID.ToString()));


            // Service
            CreateMap<TAFLDefinitionRawRow, ServiceType>()
                .ForMember(dest => dest.ServiceTypeID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<ServiceType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ServiceTypeID.ToString()));


            // Subservice
            CreateMap<TAFLDefinitionRawRow, SubserviceType>()
                .ForMember(dest => dest.SubserviceTypeID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<SubserviceType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.SubserviceTypeID.ToString()));


            // LicenseType
            CreateMap<TAFLDefinitionRawRow, LicenseType>()
                .ForMember(dest => dest.LicenseTypeID, opt => opt.MapFrom(src => src.Code));

            CreateMap<LicenseType, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.LicenseTypeID));


            // AuthorizationStatus
            CreateMap<TAFLDefinitionRawRow, AuthorizationStatus>()
                .ForMember(dest => dest.AuthorizationStatusID, opt => opt.MapFrom(src => src.Code));

            CreateMap<AuthorizationStatus, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AuthorizationStatusID));


            // OperationalStatus
            CreateMap<TAFLDefinitionRawRow, OperationalStatus>()
                .ForMember(dest => dest.OperationalStatusID, opt => opt.MapFrom(src => src.Code));

            CreateMap<OperationalStatus, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.OperationalStatusID));


            // StationClass
            CreateMap<TAFLDefinitionRawRow, StationClass>()
                .ForMember(dest => dest.StationClassID, opt => opt.MapFrom(src => src.Code));

            CreateMap<StationClass, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StationClassID));


            // StationClass
            CreateMap<TAFLDefinitionRawRow, StandbyTransmitterInformation>()
                .ForMember(dest => dest.StandbyTransmitterInformationID, opt => opt.MapFrom(src => short.Parse(src.Code)));

            CreateMap<StandbyTransmitterInformation, TAFLDefinitionRawRow>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.StandbyTransmitterInformationID.ToString()));

        }

    }
}
