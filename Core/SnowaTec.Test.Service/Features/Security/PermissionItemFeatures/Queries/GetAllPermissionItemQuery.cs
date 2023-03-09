using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Queries
{
    public class GetAllPermissionItemQuery : IRequest<Response<IEnumerable<PermissionItem>>>
    {
        public class GetAllPermissionItemQueryHandler : IRequestHandler<GetAllPermissionItemQuery, Response<IEnumerable<PermissionItem>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllPermissionItemQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<PermissionItem>>> Handle(GetAllPermissionItemQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.PermissionItems.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<PermissionItem>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<PermissionItem>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<PermissionItem>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
