using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace SnowaTec.Test.Service.Features.PortalInfoFeatures.Commands
{
    public class CreatePortalInfoCommand : PortalInfo, IRequest<Response<PortalInfo>>
    {
        public class CreatePortalInfoCommandHandler : IRequestHandler<CreatePortalInfoCommand, Response<PortalInfo>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreatePortalInfoCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<PortalInfo>> Handle(CreatePortalInfoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<PortalInfo>(request);

                    _context.PortalInfos.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<PortalInfo> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PortalInfo> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
