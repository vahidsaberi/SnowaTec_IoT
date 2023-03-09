using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Queries
{
    public class FilterFieldQuery : IRequest<Response<IEnumerable<Field>>>
    {
        public string Query { get; set; }

        public class FilterFieldQueryHandler : IRequestHandler<FilterFieldQuery, Response<IEnumerable<Field>>>
        {
            private readonly IPortalDbContext _context;

            public FilterFieldQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Field>>> Handle(FilterFieldQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Field, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Fields.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Field>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Field>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Field>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
