using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionFeatures.Queries
{
    public class FilterPermissionQuery : IRequest<Response<IEnumerable<Permission>>>
    {
        public string Query { get; set; }

        public class FilterPermissionQueryHandler : IRequestHandler<FilterPermissionQuery, Response<IEnumerable<Permission>>>
        {
            private readonly IPortalDbContext _context;

            public FilterPermissionQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Permission>>> Handle(FilterPermissionQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Permission, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Permissions.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Permission>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Permission>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Permission>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
