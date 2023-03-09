using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Commands
{
    public class DeleteUserByIdCommand : IRequest<Response<ApplicationUser>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, Response<ApplicationUser>>
        {
            private readonly IdentityContext _context;

            public DeleteUserByIdCommandHandler(IdentityContext context)
            {
                _context = context;
            }

            public async Task<Response<ApplicationUser>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Users.FindAsync(request.Id);
                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = true;
                            _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            break;
                        case 2:
                            model.Deleted = true;
                            _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            break;
                        case 3:
                            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.ApplicationUserId == model.Id).ToList());
                            _context.Users.Remove(model);
                            break;
                        case 4:
                            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.ApplicationUserId == model.Id).ToList());
                            _context.Users.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<ApplicationUser> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
