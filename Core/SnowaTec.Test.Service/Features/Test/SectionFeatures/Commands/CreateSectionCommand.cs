using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Test;
using SnowaTec.Test.Persistence.Test;

namespace SnowaTec.Test.Service.Features.Test.SectionFeatures.Commands
{
    public class CreateSectionCommand : Section, IRequest<Response<Section>>
    {
        public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, Response<Section>>
        {
            private readonly ITestDbContext _context;
            private readonly IMapper _mapper;

            public CreateSectionCommandHandler(ITestDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Section>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Section>(request);
                    _context.Sections.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Section> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Section> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
