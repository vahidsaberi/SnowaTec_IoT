using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Commands
{
    public class UpdatePermissionItemCommand : PermissionItem, IRequest<Response<PermissionItem>>
    {
        public class UpdatePermissionItemCommandHandler : IRequestHandler<UpdatePermissionItemCommand, Response<PermissionItem>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdatePermissionItemCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<PermissionItem>> Handle(UpdatePermissionItemCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.PermissionItems.FindAsync(request.Id);

                    if (model == null) return new Response<PermissionItem> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<PermissionItem, PermissionItem>(request, model);

                    _context.SetModifiedState(model);

                    _context.PermissionItems.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<PermissionItem> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PermissionItem> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
