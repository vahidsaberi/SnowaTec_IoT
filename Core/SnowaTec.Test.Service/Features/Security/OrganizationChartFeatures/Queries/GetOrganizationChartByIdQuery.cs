using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Queries
{
    public class GetOrganizationChartByIdQuery : IRequest<Response<OrganizationChart>>
    {
        public long Id { get; set; }

        public class GetOrganizationChartByIdQueryHandler : IRequestHandler<GetOrganizationChartByIdQuery, Response<OrganizationChart>>
        {
            private readonly IPortalDbContext _context;

            public GetOrganizationChartByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<OrganizationChart>> Handle(GetOrganizationChartByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.OrganizationCharts.FindAsync(request.Id);

                    if (model == null) return new Response<OrganizationChart> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<OrganizationChart> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<OrganizationChart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
