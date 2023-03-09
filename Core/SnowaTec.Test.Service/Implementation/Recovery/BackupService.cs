using MediatR;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Service.Contract.Recovery;
using SnowaTec.Test.Service.Features.Recovery.BackupFeatures.Commands;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Implementation.Recovery
{
    public class BackupService : IBackupService
    {
        private readonly IPortalDbContext _portalContext;
        private readonly IMediator _mediator;

        public BackupService(IPortalDbContext portalContext, IMediator mediator)
        {
            _portalContext = portalContext;
            _mediator = mediator;
        }

        public async Task Save(string schema, string tableName, long key, dynamic data, ActionType actionType)
        {
            var portalInfo = await _portalContext.PortalInfos.FirstOrDefaultAsync();

            if (portalInfo != null && portalInfo.RetentionOfPreviousData)
            {
                var result = await _mediator.Send(new CreateBackupCommand { Schema = schema, TableName = tableName, Key = data.Id, Data = data, ActionType = actionType  });
            }
        }
    }
}
