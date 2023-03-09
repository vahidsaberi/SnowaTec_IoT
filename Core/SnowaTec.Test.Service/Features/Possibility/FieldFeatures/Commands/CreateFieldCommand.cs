using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Commands
{
    public class CreateFieldCommand : Field, IRequest<Response<Field>>
    {
        public class CreateFieldCommandHandler : IRequestHandler<CreateFieldCommand, Response<Field>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateFieldCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Field>> Handle(CreateFieldCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<Field>(request);

                    _context.Fields.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<Field> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Field> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
