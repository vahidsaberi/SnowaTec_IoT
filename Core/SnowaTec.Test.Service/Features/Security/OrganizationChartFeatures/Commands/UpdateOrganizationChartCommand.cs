using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Commands
{
    public class UpdateOrganizationChartCommand : OrganizationChart, IRequest<Response<OrganizationChart>>
    {
        public class UpdateOrganizationChartCommandHandler : IRequestHandler<UpdateOrganizationChartCommand, Response<OrganizationChart>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public UpdateOrganizationChartCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Response<OrganizationChart>> Handle(UpdateOrganizationChartCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await _context.OrganizationCharts.FindAsync(request.Id);

                    if (model == null) return new Response<OrganizationChart> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    model = _mapper.Map<OrganizationChart, OrganizationChart>(request, model);

                    _context.SetModifiedState(model);

                    _context.OrganizationCharts.Update(model);
                    await _context.SaveChangesAsync();

                    return new Response<OrganizationChart> { Data = model, Message = "بروزرسانی اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<OrganizationChart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
