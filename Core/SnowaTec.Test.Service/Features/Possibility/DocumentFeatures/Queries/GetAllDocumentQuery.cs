using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Queries
{
    public class GetAllDocumentQuery : IRequest<Response<IEnumerable<Document>>>
    {
        public bool LoadThumbnail { get; set; }

        public class GetAllDocumentQueryHandler : IRequestHandler<GetAllDocumentQuery, Response<IEnumerable<Document>>>
        {
            private readonly IPortalDbContext _context;

            public GetAllDocumentQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Document>>> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Documents.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<Document>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    if(request.LoadThumbnail)
                    {
                        modelList.ForEach(x => { x.Content = ImageHelper.ImageToByteArray(ImageHelper.GetThumbnail(x.Content), ImageFormat.Jpeg); });
                    }

                    return new Response<IEnumerable<Document>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<Document>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
