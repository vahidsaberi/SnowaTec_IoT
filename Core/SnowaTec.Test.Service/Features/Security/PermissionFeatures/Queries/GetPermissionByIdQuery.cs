using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionFeatures.Queries
{
    public class GetPermissionByIdQuery : IRequest<Response<Permission>>
    {
        public long Id { get; set; }

        public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, Response<Permission>>
        {
            private readonly IPortalDbContext _context;

            public GetPermissionByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Permission>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Permissions.FindAsync(request.Id);

                    if (model == null) return new Response<Permission> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Permission> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Permission> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
