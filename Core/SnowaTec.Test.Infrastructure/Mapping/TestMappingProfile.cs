using SnowaTec.Test.Domain.Dtos.Test;
using SnowaTec.Test.Domain.Entities.Test;

namespace SnowaTec.Test.Infrastructure.Mapping
{
    public class TestMappingProfile : AutoMapper.Profile
    {
        public TestMappingProfile()
        {
            CreateMap<Section, Section>();
            CreateMap<Section, SectionDto>().ReverseMap();

            CreateMap<Device, Device>();
            CreateMap<Device, DeviceDto>().ReverseMap();
        }
    }
}
