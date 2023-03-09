using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionFeatures.Commands
{
    public class UpdatePermissionCommand : Permission, IRequest<Response<Permission>>
    {
        public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Response<Permission>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdatePermissionCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<Permission>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Permissions.FindAsync(request.Id);

                    if (model == null) return new Response<Permission> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<Permission, Permission>(request, model);

                    _context.SetModifiedState(model);

                    _context.Permissions.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Permission> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Permission> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
