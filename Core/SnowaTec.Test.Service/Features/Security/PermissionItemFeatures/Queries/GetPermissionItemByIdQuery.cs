using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Queries
{
    public class GetPermissionItemByIdQuery : IRequest<Response<PermissionItem>>
    {
        public long Id { get; set; }

        public class GetPermissionItemByIdQueryHandler : IRequestHandler<GetPermissionItemByIdQuery, Response<PermissionItem>>
        {
            private readonly IPortalDbContext _context;

            public GetPermissionItemByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<PermissionItem>> Handle(GetPermissionItemByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.PermissionItems.FindAsync(request.Id);

                    if (model == null) return new Response<PermissionItem> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<PermissionItem> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PermissionItem> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
