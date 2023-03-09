using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Commands
{
    public class UndeleteClientByIdCommand : IRequest<Response<Client>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteClientByIdCommandHandler : IRequestHandler<UndeleteClientByIdCommand, Response<Client>>
        {
            private readonly IPortalDbContext _context;

            public UndeleteClientByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Client>> Handle(UndeleteClientByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Clients.FindAsync(request.Id);

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

                    return new Response<Client> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Client> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
