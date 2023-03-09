using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;
using SnowaTec.Test.Service.Contract.Possibility;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Commands
{
    public class UndeleteSectionByIdCommand : IRequest<Response<Section>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteSectionByIdCommandHandler : IRequestHandler<UndeleteSectionByIdCommand, Response<Section>>
        {
            private readonly ITestDbContext _context;
            private readonly IDateTimeService _dateTimeService;

            public UndeleteSectionByIdCommandHandler(ITestDbContext context, IDateTimeService dateTimeService)
            {
                _context = context;
                _dateTimeService = dateTimeService;
            }

            public async Task<Response<Section>> Handle(UndeleteSectionByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Sections.FindAsync(request.Id);

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

                    return new Response<Section> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Section> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
