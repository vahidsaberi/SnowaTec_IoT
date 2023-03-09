using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Queries
{
    public class GetSystemPartByIdQuery : IRequest<Response<SystemPart>>
    {
        public long Id { get; set; }

        public class GetSystemPartByIdQueryHandler : IRequestHandler<GetSystemPartByIdQuery, Response<SystemPart>>
        {
            private readonly IPortalDbContext _context;

            public GetSystemPartByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<SystemPart>> Handle(GetSystemPartByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var SystemPart = await _context.SystemParts.FindAsync(request.Id);

                    if (SystemPart == null) return new Response<SystemPart> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<SystemPart> { Data = SystemPart, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<SystemPart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
