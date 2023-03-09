using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Portal.ClientFeatures.Queries
{
    public class GetAllClientQuery : IRequest<Response<IEnumerable<Client>>>
    {
        public class GetAllClientQueryHandler : IRequestHandler<GetAllClientQuery, Response<IEnumerable<Client>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllClientQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Client>>> Handle(GetAllClientQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Clients.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Client>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Client>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Client>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
