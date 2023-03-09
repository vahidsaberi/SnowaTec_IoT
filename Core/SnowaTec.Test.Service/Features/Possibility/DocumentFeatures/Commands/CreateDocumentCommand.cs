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
    public class CreateDocumentCommand : Document, IRequest<Response<Document>>
    {
        public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Response<Document>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateDocumentCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Document>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Document>(request);
                    _context.Documents.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Document> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Document> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
