using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.DeviceFeatures.Queries
{
    public class FilterDeviceQuery : IRequest<Response<IEnumerable<Device>>>
    {
        public string Query { get; set; }

        public class FilterDeviceQueryHandler : IRequestHandler<FilterDeviceQuery, Response<IEnumerable<Device>>>
        {
            private readonly ITestDbContext _context;

            public FilterDeviceQueryHandler(ITestDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Device>>> Handle(FilterDeviceQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Device, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Devices.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Device>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Device>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Device>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
