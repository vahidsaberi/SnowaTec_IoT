using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Commands
{
    public class UndeleteDocumentByIdCommand : IRequest<Response<Document>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteDocumentByIdCommandHandler : IRequestHandler<UndeleteDocumentByIdCommand, Response<Document>>
        {
            private readonly IPortalDbContext _context;

            public UndeleteDocumentByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Document>> Handle(UndeleteDocumentByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Documents.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = false;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = false;
                            await _context.SetModifiedState(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Document> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Document> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
