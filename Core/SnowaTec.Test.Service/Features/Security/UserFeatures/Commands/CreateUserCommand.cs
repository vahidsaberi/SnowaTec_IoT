using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Commands
{
    public class CreateUserCommand : ApplicationUser, IRequest<Response<ApplicationUser>>
    {
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<ApplicationUser>>
        {
            private readonly IdentityContext _context;
            private readonly IMapper _mapper;

            public CreateUserCommandHandler(IdentityContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<ApplicationUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<ApplicationUser>(request);
                    _context.Users.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<ApplicationUser> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
