using AutoMapper;
using SnowaTec.Test.Domain.DTO.Recovery;
using SnowaTec.Test.Domain.Entities.Recovery;

namespace SnowaTec.Test.Infrastructure.Mapping
{
    public class RecoveryMappingProfile : Profile
    {
        public RecoveryMappingProfile()
        {
            CreateMap<Backup, Backup>();

            #region DTO
            CreateMap<Backup, BackupDto>().ReverseMap();
            #endregion

        }
    }
}
