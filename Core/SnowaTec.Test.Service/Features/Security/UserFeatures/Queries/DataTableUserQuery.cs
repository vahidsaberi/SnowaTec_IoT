using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity;

namespace SnowaTec.Test.Service.Features.Security.UserFeatures.Queries
{
    public class DataTableUserQuery : IRequest<Response<DTResult<ApplicationUser>>>
    {
        public DTParameters Parameters { get; set; }

        public class DataTableApplicationUserQueryHandler : IRequestHandler<DataTableUserQuery, Response<DTResult<ApplicationUser>>>
        {
            private readonly IdentityContext _context;

            public DataTableApplicationUserQueryHandler(IdentityContext context)
            {
                _context = context;
            }

            public async Task<Response<DTResult<ApplicationUser>>> Handle(DataTableUserQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    //var skip = request.Parameters.Start * request.Parameters.Length;

                    var allDataCount = _context.Users.Count();

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
                        var listOfFields = string.Join('+', _context.Users.Select(x => nameof(x)));

                        query += (query.Length > 0 ? " AND " : "") + $"({listOfFields}) like CONCAT('%',N'{request.Parameters.Search.Value}','%')";
                    }

                    var schema = _context.Users.EntityType.GetSchema() ?? "dbo";
                    var tableName = _context.Users.EntityType.GetTableName();

                    query = $"SELECT * FROM [{schema}].[{tableName}] {(query.Length > 0 ? "WHERE " + query : "")}";// ORDER BY {request.Parameters.SortOrder}";

                    var modelList = _context.Users.FromSqlRaw(query);

                    var filterCount = modelList.Count();

                    var result = await modelList.Skip(request.Parameters.Start).Take(request.Parameters.Length).ToListAsync();

                    if (modelList == null) return new Response<DTResult<ApplicationUser>> { Succeeded = false, Errors = new List<string> { "اطلاعات مورد نظر پیدا نشد." } };

                    var data = new DTResult<ApplicationUser>
                    {
                        data = result,
                        draw = request.Parameters.Draw,
                        recordsTotal = allDataCount,
                        recordsFiltered = filterCount
                    };

                    return new Response<DTResult<ApplicationUser>> { Data = data, Message = "بارگذاری اطلاعات با موفقیت انجام شد.", Succeeded = true };
                }
                catch (System.Exception ex)
                {
                    return new Response<DTResult<ApplicationUser>> { Succeeded = false, Errors = new List<string> { ex.Message } };
                }
            }
        }
    }
}
