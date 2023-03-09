using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Commands
{
    public class UpdateSystemPartCommand : SystemPart, IRequest<Response<SystemPart>>
    {
        public class UpdateSystemPartCommandHandler : IRequestHandler<UpdateSystemPartCommand, Response<SystemPart>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateSystemPartCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<SystemPart>> Handle(UpdateSystemPartCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.SystemParts.FindAsync(request.Id);

                    if (model == null) return new Response<SystemPart> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<SystemPart, SystemPart>(request, model);

                    _context.SetModifiedState(model);

                    _context.SystemParts.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<SystemPart> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<SystemPart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
