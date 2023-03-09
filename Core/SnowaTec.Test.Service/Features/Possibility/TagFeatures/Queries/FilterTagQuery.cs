using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.TagFeatures.Queries
{
    public class FilterTagQuery : IRequest<Response<IEnumerable<Tag>>>
    {
        public string Query { get; set; }

        public class FilterTagQueryHandler : IRequestHandler<FilterTagQuery, Response<IEnumerable<Tag>>>
        {
            private readonly IPortalDbContext _context;

            public FilterTagQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Tag>>> Handle(FilterTagQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Tag, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Tags.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Tag>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Tag>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Tag>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
