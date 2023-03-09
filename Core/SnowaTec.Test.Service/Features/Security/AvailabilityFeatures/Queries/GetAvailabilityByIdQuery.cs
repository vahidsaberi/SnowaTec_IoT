using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Queries
{
    public class GetAvailabilityByIdQuery : IRequest<Response<Availability>>
    {
        public long Id { get; set; }

        public class GetAvailabilityByIdQueryHandler : IRequestHandler<GetAvailabilityByIdQuery, Response<Availability>>
        {
            private readonly IPortalDbContext _context;

            public GetAvailabilityByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Availability>> Handle(GetAvailabilityByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Availabilities.FindAsync(request.Id);

                    if (model == null) return new Response<Availability> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Availability> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Availability> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
