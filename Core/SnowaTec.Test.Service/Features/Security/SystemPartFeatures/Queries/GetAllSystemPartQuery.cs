using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Queries
{
    public class GetAllSystemPartQuery : IRequest<Response<IEnumerable<SystemPart>>>
    {
        public class GetAllSystemPartQueryHandler : IRequestHandler<GetAllSystemPartQuery, Response<IEnumerable<SystemPart>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllSystemPartQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<SystemPart>>> Handle(GetAllSystemPartQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var SystemPartList = await _context.SystemParts.ToListAsync();

                    if (SystemPartList == null) return new Response<IEnumerable<SystemPart>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<SystemPart>> { Data = SystemPartList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<SystemPart>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
