using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Queries
{
    public class GetClientByIdQuery : IRequest<Response<Client>>
    {
        public long Id { get; set; }

        public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Response<Client>>
        {
            private readonly IPortalDbContext _context;

            public GetClientByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Client>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Clients.FindAsync(request.Id);

                    if (model == null) return new Response<Client> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Client> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Client> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
