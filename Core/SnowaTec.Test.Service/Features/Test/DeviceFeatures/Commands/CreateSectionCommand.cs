using AutoMapper;
using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.DeviceFeatures.Commands
{
    public class CreateDeviceCommand : Device, IRequest<Response<Device>>
    {
        public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Response<Device>>
        {
            private readonly ITestDbContext _context;
            private readonly IMapper _mapper;

            public CreateDeviceCommandHandler(ITestDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Device>> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Device>(request);
                    _context.Devices.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Device> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Device> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
