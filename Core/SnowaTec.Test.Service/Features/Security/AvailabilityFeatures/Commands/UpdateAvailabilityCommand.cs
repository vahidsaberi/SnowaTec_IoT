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
    public class UpdateAvailabilityCommand : Availability, IRequest<Response<Availability>>
    {
        public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateAvailabilityCommand, Response<Availability>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateAvailabilityCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Availability>> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Availabilities.FindAsync(request.Id);

                    if (model == null) return new Response<Availability> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<Availability, Availability>(request, model);

                    _context.SetModifiedState(model);

                    _context.Availabilities.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Availability> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Availability> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
