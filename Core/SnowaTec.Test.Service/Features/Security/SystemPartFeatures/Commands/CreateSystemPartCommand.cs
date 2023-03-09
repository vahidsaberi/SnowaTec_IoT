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
    public class CreateSystemPartCommand : SystemPart, IRequest<Response<SystemPart>>
    {
        public class CreateSystemPartCommandHandler : IRequestHandler<CreateSystemPartCommand, Response<SystemPart>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateSystemPartCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<SystemPart>> Handle(CreateSystemPartCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<SystemPart>(request);
                    _context.SystemParts.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<SystemPart> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<SystemPart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
