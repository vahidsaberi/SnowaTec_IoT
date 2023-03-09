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
    public class CreatePermissionItemCommand : PermissionItem, IRequest<Response<PermissionItem>>
    {
        public class CreatePermissionItemCommandHandler : IRequestHandler<CreatePermissionItemCommand, Response<PermissionItem>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreatePermissionItemCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<PermissionItem>> Handle(CreatePermissionItemCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<PermissionItem>(request);
                    _context.PermissionItems.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<PermissionItem> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PermissionItem> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
