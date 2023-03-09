using AutoMapper;
using SnowaTec.Test.Domain.DTO.Possibility;
using SnowaTec.Test.Domain.DTO.Security;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Commands;

namespace SnowaTec.Test.Infrastructure.Mapping
{
    public class PortalMappingProfile : Profile
    {
        public PortalMappingProfile()
        {
            CreateMap<PortalInfo, PortalInfo>();
            CreateMap<Document, Document>();
            CreateMap<Document, DocumentDto>().ReverseMap();
            CreateMap<Document, UpdateDocumentCommand>().ReverseMap();

            CreateMap<SystemPart, SystemPart>();
            CreateMap<Availability, Availability>();
            CreateMap<ApplicationUser, ApplicationUser>();
            CreateMap<ApplicationUser, AuthenticationResponse>();

            CreateMap<Field, Field>();
            CreateMap<Tag, Tag>();

            CreateMap<Permission, Permission>();
            CreateMap<PermissionItem, PermissionItem>();

            #region DTO
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<PortalInfo, PortalInfoDto>().ReverseMap();

            CreateMap<Field, FieldDto>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();

            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<PermissionItem, PermissionItemDto>().ReverseMap();
            #endregion

        }
    }
}
