using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Commands
{
    public class UndeletePermissionItemByIdCommand : IRequest<Response<PermissionItem>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeletePermissionItemByIdCommandHandler : IRequestHandler<UndeletePermissionItemByIdCommand, Response<PermissionItem>>
        {
            private readonly IPortalDbContext _context;

            public UndeletePermissionItemByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<PermissionItem>> Handle(UndeletePermissionItemByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.PermissionItems.FindAsync(request.Id);

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

                    return new Response<PermissionItem> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PermissionItem> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
