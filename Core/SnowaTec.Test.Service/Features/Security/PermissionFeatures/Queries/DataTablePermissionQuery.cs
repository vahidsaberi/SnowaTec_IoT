using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal;

namespace SnowaTec.Test.Service.Features.Security.PermissionFeatures.Queries
{
    public class DataTablePermissionQuery : IRequest<Response<DTResult<Permission>>>
    {
        public DTParameters Parameters { get; set; }

        public class DataTablePermissionQueryHandler : IRequestHandler<DataTablePermissionQuery, Response<DTResult<Permission>>>
        {
            private readonly IPortalDbContext _context;

            public DataTablePermissionQueryHandler(IPortalDbContext context)
            {
                _context = context;
            }

            public async Task<Response<DTResult<Permission>>> Handle(DataTablePermissionQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    //var skip = request.Parameters.Start * request.Parameters.Length;

                    var allDataCount = _context.Permissions.Count();

                    var query = string.Empty;
                    var filters = new List<string>();

                    for (int i = 1; i < request.Parameters.Columns.Count() - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(request.Parameters.Columns[i].Search.Value))
                        {
                            if (request.Parameters.Columns[i].Data.Contains("Id"))
                                filters.Add($"{request.Parameters.Columns[i].Data} = {request.Parameters.Columns[i].Search.Value}");
                            else if (request.Parameters.Columns[i].Search.Value.IndexOf("FILTER-") == 0)
                                filters.Add($"{request.Parameters.Columns[i].Data} = {request.Parameters.Columns[i].Search.Value.Replace("FILTER-", "")}");
                            else
                                filters.Add($"{request.Parameters.Columns[i].Data} like CONCAT('%',N'{request.Parameters.Columns[i].Search.Value}','%')");
                        }
                    }

                    if (filters.Count > 0)
                    {
                        query = string.Join(" AND ", filters);
                    }

                    if (!string.IsNullOrWhiteSpace(request.Parameters.Query))
                    {
                        query += (query.Length > 0 ? " AND " : "") + request.Parameters.Query;
                    }

                    if (!string.IsNullOrWhiteSpace(request.Parameters.Search.Value))
                    {
                        var listOfFields = string.Join('+', _context.Permissions.Select(x => nameof(x)));

                        query += (query.Length > 0 ? " AND " : "") + $"({listOfFields}) like CONCAT('%',N'{request.Parameters.Search.Value}','%')";
                    }

                    var schema = _context.Permissions.EntityType.GetSchema() ?? "dbo";
                    var tableName = _context.Permissions.EntityType.GetTableName();

                    query = $"SELECT * FROM [{schema}].[{tableName}] {(query.Length > 0 ? "WHERE " + query : "")}";// ORDER BY {request.Parameters.SortOrder}";

                    var modelList = _context.Permissions.FromSqlRaw(query);

                    var filterCount = modelList.Count();

                    var result = await modelList.Skip(request.Parameters.Start).Take(request.Parameters.Length).ToListAsync();

                    if (modelList == null) return new Response<DTResult<Permission>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    var data = new DTResult<Permission>
                    {
                        data = result,
                        draw = request.Parameters.Draw,
                        recordsTotal = allDataCount,
                        recordsFiltered = filterCount
                    };

                    return new Response<DTResult<Permission>> { Data = data, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<DTResult<Permission>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
