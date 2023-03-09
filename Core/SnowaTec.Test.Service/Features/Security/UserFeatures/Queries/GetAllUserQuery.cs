using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Queries
{
    public class GetAllUserQuery : IRequest<Response<IEnumerable<ApplicationUser>>>
    {
        public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Response<IEnumerable<ApplicationUser>>>
        {
            private readonly IdentityContext _context;

            public GetAllUserQueryHandler(IdentityContext context)
            {
                _context = context;
            }

            public async Task<Response<IEnumerable<ApplicationUser>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var modelList = await _context.Users.ToListAsync();

                    if (modelList == null) return new Response<IEnumerable<ApplicationUser>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    return new Response<IEnumerable<ApplicationUser>> { Data = modelList.AsReadOnly(), Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<IEnumerable<ApplicationUser>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
