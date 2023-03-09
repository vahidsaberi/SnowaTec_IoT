using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Queries
{
    public class GetAllFieldQuery : IRequest<Response<IEnumerable<Field>>>
    {
        public class GetAllFieldQueryHandler : IRequestHandler<GetAllFieldQuery, Response<IEnumerable<Field>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllFieldQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Field>>> Handle(GetAllFieldQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Fields.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Field>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Field>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
            }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Field>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
