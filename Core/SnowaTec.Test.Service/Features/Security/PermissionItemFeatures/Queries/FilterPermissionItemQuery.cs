using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Queries
{
    public class FilterPermissionItemQuery : IRequest<Response<IEnumerable<PermissionItem>>>
    {
        public string Query { get; set; }

        public class FilterPermissionItemQueryHandler : IRequestHandler<FilterPermissionItemQuery, Response<IEnumerable<PermissionItem>>>
        {
            private readonly IPortalDbContext _context;

            public FilterPermissionItemQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<PermissionItem>>> Handle(FilterPermissionItemQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<PermissionItem, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.PermissionItems.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<PermissionItem>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<PermissionItem>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<PermissionItem>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
