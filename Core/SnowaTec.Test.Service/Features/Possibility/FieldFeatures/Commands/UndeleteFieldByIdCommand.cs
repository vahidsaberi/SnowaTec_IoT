using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Commands
{
    public class UndeleteFieldByIdCommand : IRequest<Response<Field>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteFieldByIdCommandHandler : IRequestHandler<UndeleteFieldByIdCommand, Response<Field>>
        {
            private readonly IPortalDbContext _context;

            public UndeleteFieldByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Field>> Handle(UndeleteFieldByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Fields.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = false;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = false;
                            await _context.SetModifiedState(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Field> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Field> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
