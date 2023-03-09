using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.PortalInfoFeatures.Commands
{
    public class UpdatePortalInfoCommand : PortalInfo, IRequest<Response<PortalInfo>>
    {
        public class UpdatePortalInfoCommandHandler : IRequestHandler<UpdatePortalInfoCommand, Response<PortalInfo>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdatePortalInfoCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<PortalInfo>> Handle(UpdatePortalInfoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.PortalInfos.FindAsync(request.Id);

                    if (model == null) return new Response<PortalInfo> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<PortalInfo, PortalInfo>(request, model);

                    _context.SetModifiedState(model);

                    _context.PortalInfos.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<PortalInfo> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<PortalInfo> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
