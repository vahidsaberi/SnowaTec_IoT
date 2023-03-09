using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Queries
{
    public class GetUserByIdQuery : IRequest<Response<ApplicationUser>>
    {
        public long Id { get; set; }

        public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<ApplicationUser>>
        {
            private readonly IdentityContext _context;

            public GetUserByIdQueryHandler(IdentityContext context)
            {
                _context = context;
            }

            public async Task<Response<ApplicationUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.Users.FindAsync(request.Id);

                    if (model == null) return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<ApplicationUser> { Data = model, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<ApplicationUser> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
