using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace SnowaTec.Test.Service.Features.Possibility.TagFeatures.Commands
{
    public class CreateTagCommand : Tag, IRequest<Response<Tag>>
    {
        public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Response<Tag>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateTagCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Tag>(request);

                    _context.Tags.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Tag> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Tag> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
