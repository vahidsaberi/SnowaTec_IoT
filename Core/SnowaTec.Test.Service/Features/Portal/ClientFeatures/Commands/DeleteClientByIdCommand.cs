using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Commands
{
    public class DeleteClientByIdCommand : IRequest<Response<Client>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteClientByIdCommandHandler : IRequestHandler<DeleteClientByIdCommand, Response<Client>>
        {
            private readonly IPortalDbContext _context;

            public DeleteClientByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Client>> Handle(DeleteClientByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Clients.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 3:
                            _context.Clients.Remove(model);
                            break;
                        case 4:
                            _context.Clients.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Client> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Client> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
