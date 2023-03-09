using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Domain.Helpers;
using SnowaTec.Test.Persistence.Portal;
using System.Drawing.Imaging;

namespace SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Queries
{
    public class FilterDocumentQuery : IRequest<Response<IEnumerable<Document>>>
    {
        public string Query { get; set; }
        public bool LoadThumbnail { get; set; }

        public class FilterDocumentQueryHandler : IRequestHandler<FilterDocumentQuery, Response<IEnumerable<Document>>>
        {
            private readonly IPortalDbContext _context;

            public FilterDocumentQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<Document>>> Handle(FilterDocumentQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var expression = LinqExpressionHelper.DeserialiseRemoteExpression<Remote.Linq.Expressions.LambdaExpression>(request.Query) as Remote.Linq.Expressions.LambdaExpression;

                    var localexpression = LinqExpressionHelper.ToLinqExpression<Document, bool>(expression);

                    var predicate = localexpression.Compile();

                    var modelList = _context.Documents.Where(predicate).ToList();

                    if (modelList == null) return new Response<IEnumerable<Document>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    if (request.LoadThumbnail)
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
