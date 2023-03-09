using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Commands
{
    public class UpdateFieldCommand : Field, IRequest<Response<Field>>
    {
        public class UpdateFieldCommandHandler : IRequestHandler<UpdateFieldCommand, Response<Field>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateFieldCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<Field>> Handle(UpdateFieldCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Fields.FindAsync(request.Id);

                    if (model == null) return new Response<Field> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<Field, Field>(request, model);

                    _context.SetModifiedState(model);

                    _context.Fields.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<Field> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<Field> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
