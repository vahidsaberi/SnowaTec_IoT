using AutoMapper;
using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Commands
{
    public class UpdateClientCommand : Client, IRequest<Response<Client>>
    {
        public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Response<Client>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateClientCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Client>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Clients.FindAsync(request.Id);

                    if (model == null) return new Response<Client> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<Client, Client>(request, model);

                    _context.SetModifiedState(model);

                    _context.Clients.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Client> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Client> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
