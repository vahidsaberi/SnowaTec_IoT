using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Domain.Helper;
using System.Drawing.Imaging;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Queries
{
    public class GetDocumentByIdQuery : IRequest<Response<Document>>
    {
        public long Id { get; set; }
        public bool LoadThumbnail { get; set; }

        public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Response<Document>>
        {
            private readonly IPortalDbContext _context;

            public GetDocumentByIdQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Document>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Documents.FindAsync(request.Id);

                    if (model == null) return new Response<Document> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    if (request.LoadThumbnail)
                    {
                        model.Content = ImageHelper.ImageToByteArray(ImageHelper.GetThumbnail(model.Content), ImageFormat.Jpeg);
                    }

                    return new Response<Document> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Document> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
