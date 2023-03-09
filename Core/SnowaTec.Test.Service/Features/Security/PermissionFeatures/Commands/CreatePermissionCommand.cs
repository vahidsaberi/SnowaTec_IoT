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
    public class CreatePermissionCommand : Permission, IRequest<Response<Permission>>
    {
        public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Response<Permission>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreatePermissionCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Permission>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Permission>(request);
                    _context.Permissions.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Permission> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Permission> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
