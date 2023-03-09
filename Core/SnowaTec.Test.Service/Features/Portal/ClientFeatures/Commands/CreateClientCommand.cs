using AutoMapper;
using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Commands
{
    public class CreateClientCommand : Client, IRequest<Response<Client>>
    {
        public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Response<Client>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateClientCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Client>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Client>(request);
                    _context.Clients.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Client> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Client> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
