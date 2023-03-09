using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Queries
{
    public class FilterAvailabilityQuery : IRequest<Response<IEnumerable<Availability>>>
    {
        public string Query { get; set; }

        public class FilterAvailabilityQueryHandler : IRequestHandler<FilterAvailabilityQuery, Response<IEnumerable<Availability>>>
        {
            private readonly IPortalDbContext _context;

            public FilterAvailabilityQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Availability>>> Handle(FilterAvailabilityQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Availability, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Availabilities.Where(predicate).ToList();

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
