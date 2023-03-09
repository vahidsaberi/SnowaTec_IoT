using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Queries
{
    public class GetAllOrganizationChartQuery : IRequest<Response<IEnumerable<OrganizationChart>>>
    {
        public class GetAllOrganizationChartQueryHandler : IRequestHandler<GetAllOrganizationChartQuery, Response<IEnumerable<OrganizationChart>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllOrganizationChartQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<OrganizationChart>>> Handle(GetAllOrganizationChartQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.OrganizationCharts.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<OrganizationChart>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<OrganizationChart>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<OrganizationChart>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
