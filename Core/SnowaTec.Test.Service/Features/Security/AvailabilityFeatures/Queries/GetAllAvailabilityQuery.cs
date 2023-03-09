using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Queries
{
    public class GetAllAvailabilityQuery : IRequest<Response<IEnumerable<Availability>>>
    {
        public class GetAllAvailabilityQueryHandler : IRequestHandler<GetAllAvailabilityQuery, Response<IEnumerable<Availability>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllAvailabilityQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Availability>>> Handle(GetAllAvailabilityQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Availabilities.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Availability>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Availability>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Availability>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
