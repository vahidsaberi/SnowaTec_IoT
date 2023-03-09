using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.DeviceFeatures.Queries
{
    public class GetDeviceByIdQuery : IRequest<Response<Device>>
    {
        public long Id { get; set; }

        public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, Response<Device>>
        {
            private readonly ITestDbContext _context;

            public GetDeviceByIdQueryHandler(ITestDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Device>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Devices.FindAsync(request.Id);

                    if (model == null) return new Response<Device> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Device> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Device> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
