using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Commands
{
    public class UpdateDocumentCommand : Document, IRequest<Response<Document>>
    {
        public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, Response<Document>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateDocumentCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Document>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Documents.FindAsync(request.Id);

                    if (model == null) return new Response<Document> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<Document, Document>(request, model);

                    _context.SetModifiedState(model);

                    _context.Documents.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Document> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Document> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
