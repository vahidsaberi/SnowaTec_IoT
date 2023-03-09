using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Queries
{
    public class FilterOrganizationChartQuery : IRequest<Response<IEnumerable<OrganizationChart>>>
    {
        public string Query { get; set; }

        public class FilterOrganizationChartQueryHandler : IRequestHandler<FilterOrganizationChartQuery, Response<IEnumerable<OrganizationChart>>>
        {
            private readonly IPortalDbContext _context;

            public FilterOrganizationChartQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<OrganizationChart>>> Handle(FilterOrganizationChartQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<OrganizationChart, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.OrganizationCharts.Where(predicate).ToList();

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
