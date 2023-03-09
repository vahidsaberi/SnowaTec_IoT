using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Queries
{
    public class FilterSectionQuery : IRequest<Response<IEnumerable<Section>>>
    {
        public string Query { get; set; }

        public class FilterSectionQueryHandler : IRequestHandler<FilterSectionQuery, Response<IEnumerable<Section>>>
        {
            private readonly ITestDbContext _context;

            public FilterSectionQueryHandler(ITestDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Section>>> Handle(FilterSectionQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Section, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Sections.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Section>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Section>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Section>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
