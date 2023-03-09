using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Persistence.Test;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Contract.Recovery;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Test.DeviceFeatures.Commands
{
    public class UpdateDeviceCommand : Device, IRequest<Response<Device>>
    {
        public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, Response<Device>>
        {
            private readonly ITestDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBackupService _backupService;
            private readonly IDateTimeService _dateTimeService;

            public UpdateDeviceCommandHandler(ITestDbContext context, IMapper mapper, IBackupService backupService, IDateTimeService dateTimeService)
            {
                _context = context;
                _mapper = mapper;
                _backupService = backupService;
                _dateTimeService = dateTimeService;
            }

            public async Task<Response<Device>> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Devices.FindAsync(request.Id);

                    if (model == null) return new Response<Device> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    var schema = _context.Devices.EntityType.GetSchema() ?? "dbo";
                    var tableName = _context.Devices.EntityType.GetTableName();

                    await _backupService.Save(schema, tableName, model.Id, model, ActionType.Update);

                    model = _mapper.Map<Device, Device>(request, model);

                    model.UpdateRowDate = _dateTimeService.Now;

                    _context.SetModifiedState(model);

                    _context.Devices.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Device> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Device> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
