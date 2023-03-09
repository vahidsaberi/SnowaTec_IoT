using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Queries
{
    public class FilterSystemPartQuery : IRequest<Response<IEnumerable<SystemPart>>>
    {
        public string Query { get; set; }

        public class FilterSystemPartQueryHandler : IRequestHandler<FilterSystemPartQuery, Response<IEnumerable<SystemPart>>>
        {
            private readonly IPortalDbContext _context;

            public FilterSystemPartQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<SystemPart>>> Handle(FilterSystemPartQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<SystemPart, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.SystemParts.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<SystemPart>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<SystemPart>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<SystemPart>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
