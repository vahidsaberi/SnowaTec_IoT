using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionFeatures.Commands
{
    public class DeletePermissionByIdCommand : IRequest<Response<Permission>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeletePermissionByIdCommandHandler : IRequestHandler<DeletePermissionByIdCommand, Response<Permission>>
        {
            private readonly IPortalDbContext _context;

            public DeletePermissionByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Permission>> Handle(DeletePermissionByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Permissions.FindAsync(request.Id);
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
                            _context.Permissions.Remove(model);
                            break;
                        case 4:
                            _context.Permissions.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Permission> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Permission> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
