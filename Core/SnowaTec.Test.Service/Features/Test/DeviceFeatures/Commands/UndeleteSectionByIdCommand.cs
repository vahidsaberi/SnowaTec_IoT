using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;
using SnowaTec.Test.Service.Contract.Possibility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Test.DeviceFeatures.Commands
{
    public class UndeleteDeviceByIdCommand : IRequest<Response<Device>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteDeviceByIdCommandHandler : IRequestHandler<UndeleteDeviceByIdCommand, Response<Device>>
        {
            private readonly ITestDbContext _context;
            private readonly IDateTimeService _dateTimeService;

            public UndeleteDeviceByIdCommandHandler(ITestDbContext context, IDateTimeService dateTimeService)
            {
                _context = context;
                _dateTimeService = dateTimeService;
            }

            public async Task<Response<Device>> Handle(UndeleteDeviceByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Devices.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = false;
                            model.UpdateRowDate = _dateTimeService.Now;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = false;
                            model.UpdateRowDate = _dateTimeService.Now;
                            await _context.SetModifiedState(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Device> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Device> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
