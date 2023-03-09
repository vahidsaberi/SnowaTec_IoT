using MediatR;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Commands
{
    public class DeleteOrganizationChartByIdCommand : IRequest<Response<OrganizationChart>>
    {
        public long Id { get; set; }
        public int Type { get; set; }

        public class DeleteOrganizationChartByIdCommandHandler : IRequestHandler<DeleteOrganizationChartByIdCommand, Response<OrganizationChart>>
        {
            private readonly IPortalDbContext _context;

            public DeleteOrganizationChartByIdCommandHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<OrganizationChart>> Handle(DeleteOrganizationChartByIdCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.OrganizationCharts.FindAsync(request.Id);

                    if (model == null) return default;

                    switch (request.Type)
                    {
                        case 1:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 2:
                            model.Deleted = true;
                            await _context.SetModifiedState(model);
                            break;
                        case 3:
                            _context.OrganizationCharts.Remove(model);
                            break;
                        case 4:
                            _context.OrganizationCharts.Remove(model);
                            break;
                    }

                    await _context.SaveChangesAsync();

                    return new Response<OrganizationChart> { Data = model, Message = "حذف اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<OrganizationChart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
