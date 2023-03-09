using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Queries
{
    public class FilterUserQuery : IRequest<Response<IEnumerable<ApplicationUser>>>
    {
        public string Query { get; set; }

        public class FilterUserQueryHandler : IRequestHandler<FilterUserQuery, Response<IEnumerable<ApplicationUser>>>
        {
            private readonly IdentityContext _context;

            public FilterUserQueryHandler(IdentityContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<ApplicationUser>>> Handle(FilterUserQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<ApplicationUser, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Users.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<ApplicationUser>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<ApplicationUser>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<ApplicationUser>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
