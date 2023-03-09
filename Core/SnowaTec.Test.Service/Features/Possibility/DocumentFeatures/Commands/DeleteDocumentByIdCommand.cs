using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Commands
{
    public class DeleteDocumentByIdCommand : IRequest<Response<Document>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteDocumentByIdCommandHandler : IRequestHandler<DeleteDocumentByIdCommand, Response<Document>>
        {
            private readonly IPortalDbContext _context;

            public DeleteDocumentByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Document>> Handle(DeleteDocumentByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Documents.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 3:
                            _context.Documents.Remove(model);
                            break;
                        case 4:
                            _context.Documents.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Document> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Document> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
