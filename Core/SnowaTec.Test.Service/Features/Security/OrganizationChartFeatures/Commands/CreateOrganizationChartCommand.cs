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
    public class CreateOrganizationChartCommand : OrganizationChart, IRequest<Response<OrganizationChart>>
    {
        public class CreateOrganizationChartCommandHandler : IRequestHandler<CreateOrganizationChartCommand, Response<OrganizationChart>>
        {
            private readonly IPortalDbContext _context;
            private readonly IMapper _mapper;

            public CreateOrganizationChartCommandHandler(IPortalDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response<OrganizationChart>> Handle(CreateOrganizationChartCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = _mapper.Map<OrganizationChart>(request);
                    _context.OrganizationCharts.Add(model);
                    await _context.SaveChangesAsync();

                    return new Response<OrganizationChart> { Data = model, Message = "ذخیره اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<OrganizationChart> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
