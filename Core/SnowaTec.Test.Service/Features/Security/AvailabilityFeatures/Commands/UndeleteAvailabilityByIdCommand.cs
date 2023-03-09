using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Commands
{
    public class UndeleteAvailabilityByIdCommand : IRequest<Response<Availability>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteAvailabilityByIdCommandHandler : IRequestHandler<UndeleteAvailabilityByIdCommand, Response<Availability>>
        {
            private readonly IPortalDbContext _context;

            public UndeleteAvailabilityByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Availability>> Handle(UndeleteAvailabilityByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Availabilities.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = false;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = false;
                            await _context.SetModifiedState(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Availability> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Availability> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
