using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.PortalInfoFeatures.Commands
{
    public class DeletePortalInfoByIdCommand : IRequest<Response<PortalInfo>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeletePortalInfoByIdCommandHandler : IRequestHandler<DeletePortalInfoByIdCommand, Response<PortalInfo>>
        {
            private readonly IPortalDbContext _context;

            public DeletePortalInfoByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<PortalInfo>> Handle(DeletePortalInfoByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.PortalInfos.FindAsync(request.Id);

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
                            _context.PortalInfos.Remove(model);
                            break;
                        case 4:
                            _context.PortalInfos.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<PortalInfo> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PortalInfo> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
