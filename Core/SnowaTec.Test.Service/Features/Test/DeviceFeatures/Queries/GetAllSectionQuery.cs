using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.DeviceFeatures.Queries
{
    public class GetAllDeviceQuery : IRequest<Response<IEnumerable<Device>>>
    {
        public class GetAllDeviceQueryHandler : IRequestHandler<GetAllDeviceQuery, Response<IEnumerable<Device>>>
        {
            private readonly ITestDbContext _context;

            public GetAllDeviceQueryHandler(ITestDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Device>>> Handle(GetAllDeviceQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Devices.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Device>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Device>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Device>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
