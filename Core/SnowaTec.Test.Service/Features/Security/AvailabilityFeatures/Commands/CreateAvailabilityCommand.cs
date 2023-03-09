using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Commands
{
    public class CreateAvailabilityCommand : Availability, IRequest<Response<Availability>>
    {
        public class CreateAvailabilityCommandHandler : IRequestHandler<CreateAvailabilityCommand, Response<Availability>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateAvailabilityCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Availability>> Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Availability>(request);
                    _context.Availabilities.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Availability> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Availability> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
