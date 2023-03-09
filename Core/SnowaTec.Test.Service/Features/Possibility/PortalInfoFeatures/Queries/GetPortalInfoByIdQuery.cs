using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.PortalInfoFeatures.Queries
{
    public class GetPortalInfoByIdQuery : IRequest<Response<PortalInfo>>
    {
        public long Id { get; set; }

        public class GetPortalInfoByIdQueryHandler : IRequestHandler<GetPortalInfoByIdQuery, Response<PortalInfo>>
        {
            private readonly IPortalDbContext _context;

            public GetPortalInfoByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<PortalInfo>> Handle(GetPortalInfoByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.PortalInfos.FindAsync(request.Id);

                    if (model == null) return new Response<PortalInfo> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<PortalInfo> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PortalInfo> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
