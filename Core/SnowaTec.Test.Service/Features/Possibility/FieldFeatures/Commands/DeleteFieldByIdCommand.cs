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
    public class DeleteFieldByIdCommand : IRequest<Response<Field>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteFieldByIdCommandHandler : IRequestHandler<DeleteFieldByIdCommand, Response<Field>>
        {
            private readonly IPortalDbContext _context;

            public DeleteFieldByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Field>> Handle(DeleteFieldByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Fields.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 3:
                            _context.Fields.Remove(model);
                            break;
                        case 4:
                            _context.Fields.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Field> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Field> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
