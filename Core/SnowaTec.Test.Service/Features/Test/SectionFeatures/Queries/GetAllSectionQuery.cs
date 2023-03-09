using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Queries
{
    public class GetAllSectionQuery : IRequest<Response<IEnumerable<Section>>>
    {
        public class GetAllSectionQueryHandler : IRequestHandler<GetAllSectionQuery, Response<IEnumerable<Section>>>
        {
            private readonly ITestDbContext _context;

            public GetAllSectionQueryHandler(ITestDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Section>>> Handle(GetAllSectionQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Sections.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Section>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<Section>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Section>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
