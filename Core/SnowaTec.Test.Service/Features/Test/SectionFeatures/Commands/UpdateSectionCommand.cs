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

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Commands
{
    public class UpdateSectionCommand : Section, IRequest<Response<Section>>
    {
        public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, Response<Section>>
        {
            private readonly ITestDbContext _context;
            private readonly IMapper _mapper;
            private readonly IBackupService _backupService;
            private readonly IDateTimeService _dateTimeService;

            public UpdateSectionCommandHandler(ITestDbContext context, IMapper mapper, IBackupService backupService, IDateTimeService dateTimeService)
            {
                _context = context;
                _mapper = mapper;
                _backupService = backupService;
                _dateTimeService = dateTimeService;
            }

            public async Task<Response<Section>> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Sections.FindAsync(request.Id);

                    if (model == null) return new Response<Section> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    var schema = _context.Sections.EntityType.GetSchema() ?? "dbo";
                    var tableName = _context.Sections.EntityType.GetTableName();

                    await _backupService.Save(schema, tableName, model.Id, model, ActionType.Update);

                    model = _mapper.Map<Section, Section>(request, model);

                    model.UpdateRowDate = _dateTimeService.Now;

                    _context.SetModifiedState(model);

                    _context.Sections.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Section> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Section> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
