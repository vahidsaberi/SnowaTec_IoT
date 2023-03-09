using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Commands
{
    public class UndeleteUserByIdCommand : IRequest<Response<ApplicationUser>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class UndeleteUserByIdCommandHandler : IRequestHandler<UndeleteUserByIdCommand, Response<ApplicationUser>>
        {
            private readonly IdentityContext _context;

            public UndeleteUserByIdCommandHandler(IdentityContext context)
            {
                _context = context;
            }

            public async Task<Response<ApplicationUser>> Handle(UndeleteUserByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Users.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = false;
                            _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            break;
                        case 2:
                            model.Deleted = false;
                            _context.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<ApplicationUser> { Data = model, Message = "بازیافت اطلاعات حذف شده با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
