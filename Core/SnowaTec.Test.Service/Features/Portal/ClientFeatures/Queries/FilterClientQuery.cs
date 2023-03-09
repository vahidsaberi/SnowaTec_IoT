using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Queries
{
    public class FilterClientQuery : IRequest<Response<IEnumerable<Client>>>
    {
        public string Query { get; set; }

        public class FilterClientQueryHandler : IRequestHandler<FilterClientQuery, Response<IEnumerable<Client>>>
        {
            private readonly IPortalDbContext _context;

            public FilterClientQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Client>>> Handle(FilterClientQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Client, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Clients.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Client>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Client>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Client>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
