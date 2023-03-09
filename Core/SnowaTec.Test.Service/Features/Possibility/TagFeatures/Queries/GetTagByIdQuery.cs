using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.TagFeatures.Queries
{
    public class GetTagByIdQuery : IRequest<Response<Tag>>
    {
        public long Id { get; set; }

        public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Response<Tag>>
        {
            private readonly IPortalDbContext _context;

            public GetTagByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Tag>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Tags.FindAsync(request.Id);

                    if (model == null) return new Response<Tag> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Tag> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Tag> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
