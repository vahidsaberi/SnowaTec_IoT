using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Possibility.TagFeatures.Commands
{
    public class DeleteTagByIdCommand : IRequest<Response<Tag>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteTagByIdCommandHandler : IRequestHandler<DeleteTagByIdCommand, Response<Tag>>
        {
            private readonly IPortalDbContext _context;

            public DeleteTagByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<Tag>> Handle(DeleteTagByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Tags.FindAsync(request.Id);

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
                            _context.Tags.Remove(model);
                            break;
                        case 4:
                            _context.Tags.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<Tag> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Tag> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
