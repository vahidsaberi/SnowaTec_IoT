using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Queries
{
    public class GetModuleItemsCountQuery : IRequest<Response<List<long>>>
    {
        public ModuleType? ModuleId { get; set; }
        public int? ModuleRecordId { get; set; }
        public int? ModuleSubRecordId { get; set; }

        public class GetModuleItemsCountQueryHandler : IRequestHandler<GetModuleItemsCountQuery, Response<List<long>>>
        {
            private readonly IPortalDbContext _context;

            public GetModuleItemsCountQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<long>>> Handle(GetModuleItemsCountQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = new List<long>();

                    var items = await _context.Documents.Select(x => new { x.Id, x.ModuleId, x.ModuleRecordId, x.ModuleSubRecordId }).ToListAsync();

                    if (request.ModuleId != null)
                    {
                        items = items.Where(x => x.ModuleId == request.ModuleId).ToList();
                    }

                    if (request.ModuleRecordId != null)
                    {
                        items = items.Where(x => x.ModuleRecordId == request.ModuleRecordId).ToList();
                    }

                    if (request.ModuleSubRecordId != null)
                    {
                        items = items.Where(x => x.ModuleSubRecordId == request.ModuleSubRecordId).ToList();
                    }

                    model = items.Select(x => x.Id).ToList();

                    if (model == null) return new Response<List<long>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<List<long>> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<List<long>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
