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

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Commands
{
    public class DeleteSectionByIdCommand : IRequest<Response<Section>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteSectionByIdCommandHandler : IRequestHandler<DeleteSectionByIdCommand, Response<Section>>
        {
            private readonly ITestDbContext _context;
            private readonly IBackupService _backupService;
            private readonly IDateTimeService _dateTimeService;

            public DeleteSectionByIdCommandHandler(ITestDbContext context, IBackupService backupService, IDateTimeService dateTimeService)
            {
                _context = context;
                _backupService = backupService;
                _dateTimeService = dateTimeService;
            }

            public async Task<Response<Section>> Handle(DeleteSectionByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Sections.FindAsync(request.Id);

                    if (model == null) return default;

                    var schema = _context.Sections.EntityType.GetSchema() ?? "dbo";
                    var tableName = _context.Sections.EntityType.GetTableName();

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = true;
                            model.UpdateRowDate = _dateTimeService.Now;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = true;
                            model.UpdateRowDate = _dateTimeService.Now;
                            await _context.SetModifiedState(model);
                            break;
                        case 3:
                            await _backupService.Save(schema, tableName, model.Id, model, ActionType.Delete);
                            _context.Sections.Remove(model);
                            break;
                        case 4:
                            await _backupService.Save(schema, tableName, model.Id, model, ActionType.Delete);
                            _context.Sections.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Section> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Section> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
