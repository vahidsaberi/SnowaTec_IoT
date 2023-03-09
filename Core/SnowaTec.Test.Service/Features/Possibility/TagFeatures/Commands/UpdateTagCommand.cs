using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.TagFeatures.Commands
{
    public class UpdateTagCommand : Tag, IRequest<Response<Tag>>
    {
        public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Response<Tag>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateTagCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Tag>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Tags.FindAsync(request.Id);

                    if (model == null) return new Response<Tag> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<Tag, Tag>(request, model);

                    _context.SetModifiedState(model);

                    _context.Tags.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Tag> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Tag> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
