using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Queries
{
    public class GetFieldByIdQuery : IRequest<Response<Field>>
    {
        public long Id { get; set; }

        public class GetFieldByIdQueryHandler : IRequestHandler<GetFieldByIdQuery, Response<Field>>
        {
            private readonly IPortalDbContext _context;

            public GetFieldByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Field>> Handle(GetFieldByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Fields.FindAsync(request.Id);

                    if (model == null) return new Response<Field> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<Field> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Field> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
