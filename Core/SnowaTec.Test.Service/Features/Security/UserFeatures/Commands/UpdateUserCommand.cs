using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Commands
{
    public class UpdateUserCommand : ApplicationUser, IRequest<Response<ApplicationUser>>
    {
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<ApplicationUser>>
        {
            private readonly IdentityContext _context;
            private readonly IMapper _mapper;

            public UpdateUserCommandHandler(IdentityContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<ApplicationUser>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Users.FindAsync(request.Id);

                    if (model == null) return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<ApplicationUser, ApplicationUser>(request, model);

                    _context.Entry(model).State = EntityState.Modified;

                    _context.Users.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<ApplicationUser> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
