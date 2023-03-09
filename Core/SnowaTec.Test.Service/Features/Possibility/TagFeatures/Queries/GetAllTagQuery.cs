using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Possibility.TagFeatures.Queries
{
    public class GetAllTagQuery : IRequest<Response<IEnumerable<Tag>>>
    {
        public class GetAllTagQueryHandler : IRequestHandler<GetAllTagQuery, Response<IEnumerable<Tag>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllTagQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Tag>>> Handle(GetAllTagQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Tags.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Tag>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Tag>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
            }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Tag>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
