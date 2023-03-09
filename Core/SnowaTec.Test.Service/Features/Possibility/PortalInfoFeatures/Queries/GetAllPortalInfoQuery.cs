using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.PortalInfoFeatures.Queries
{
    public class GetAllPortalInfoQuery : IRequest<Response<IEnumerable<PortalInfo>>>
    {
        public class GetAllPortalInfoQueryHandler : IRequestHandler<GetAllPortalInfoQuery, Response<IEnumerable<PortalInfo>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllPortalInfoQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<PortalInfo>>> Handle(GetAllPortalInfoQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.PortalInfos.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<PortalInfo>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<PortalInfo>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
            }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<PortalInfo>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
