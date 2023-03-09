using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Queries
{
    public class GetSectionByIdQuery : IRequest<Response<Section>>
    {
        public long Id { get; set; }

        public class GetSectionByIdQueryHandler : IRequestHandler<GetSectionByIdQuery, Response<Section>>
        {
            private readonly ITestDbContext _context;

            public GetSectionByIdQueryHandler(ITestDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Section>> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Sections.FindAsync(request.Id);

                    if (model == null) return new Response<Section> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Section> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Section> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
