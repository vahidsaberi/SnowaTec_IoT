using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Commands
{
    public class DeleteSystemPartByIdCommand : IRequest<Response<SystemPart>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteSystemPartByIdCommandHandler : IRequestHandler<DeleteSystemPartByIdCommand, Response<SystemPart>>
        {
            private readonly IPortalDbContext _context;

            public DeleteSystemPartByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<SystemPart>> Handle(DeleteSystemPartByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.SystemParts.FindAsync(request.Id);

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
                            _context.SystemParts.Remove(model);
                            break;
                        case 4:
                            _context.SystemParts.Remove(model);
                            break;
                    }
                    await _context.SaveChangesAsync();

                    return new Response<SystemPart> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<SystemPart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
