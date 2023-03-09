using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionFeatures.Queries
{
    public class GetAllPermissionQuery : IRequest<Response<IEnumerable<Permission>>>
    {
        public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, Response<IEnumerable<Permission>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllPermissionQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Permission>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Permissions.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Permission>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Permission>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Permission>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
