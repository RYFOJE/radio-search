using AutoMapper;
using NetTopologySuite.Geometries;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Mappings
{
    public class TAFLRowProfile : Profile
    {
        public TAFLRowProfile()
        {
            CreateMap<TAFLEntryRawRow, LicenseRecord>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                    src.Longitude.HasValue && src.Latitude.HasValue
                        ? new Point((double)src.Longitude.Value, (double)src.Latitude.Value) { SRID = 4326 }
                        : null))
                .ForMember(dest => dest.CanadaLicenseRecordID, opt => opt.MapFrom(src => src.LicenseRecordID))
                .ForMember(dest => dest.IsValid, opt => opt.Ignore());
        }
    }
}
