using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Recovery;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Recovery;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;
using System.Reflection;

namespace SnowaTec.Test.Web.Pages.Admin.Recovery
{
    [Authorize]
    public class IndexModel : PageModel
    {
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        public string Schema { get; set; }
        public string TableName { get; set; }

        public IndexModel(ICallApiService service, IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }

        public async Task OnGet(string schema, string tableName)
        {
            Schema = schema;
            TableName = tableName;
        }

        public async Task<JsonResult> OnPostAsync(DTParameters dtModel, string schema, string tableName)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            dtModel.Query = $"[Schema] = '{schema}' AND [TableName] = '{tableName}'";

            var result = await _service.Call<DTResult<Backup>>(token, ApiRoutes.Backups.Pagination, CallMethodType.Post, dtModel);

            var model = _mapper.Map<List<BackupDto>>(result.Data.data);

            return new JsonResult(new DTResult<BackupDto>
            {
                data = model.OrderBy(x=>x.Key).ThenBy(x=>x.CreateRowDate).ToList(),
                draw = result.Data.draw,
                recordsFiltered = result.Data.recordsFiltered,
                recordsTotal = result.Data.recordsTotal
            });
        }

        public async Task<ActionResult> OnPostDeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Backup>(token, ApiRoutes.Backups.Delete, CallMethodType.Delete, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Backup> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUndeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Backup>(token, ApiRoutes.Backups.UnDelete, CallMethodType.Post, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Backup> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }


        public async Task<ActionResult> OnPostRecoveryItemAsync(long id)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var result = await _service.Call<Backup>(token, ApiRoutes.Backups.Get, CallMethodType.Get, null, ("Id", id));

            if(result.Data != null)
            {
                var schema = result.Data.Schema;
                var tableName = result.Data.TableName;
                var data = result.Data.Data;

                var routes = typeof(ApiRoutes.Recoveries).GetFields();

                foreach (var item in routes.Where(x=>x.Name.Contains("ExistTabel")).ToList())
                {
                    var rout = item.GetValue(typeof(ApiRoutes.Recoveries)).ToString();

                    var existTable = await _service.Call<bool>(token, rout, CallMethodType.Get, null, ("schema", schema), ("tableName", tableName));

                    if(existTable.Data)
                    {
                        var name = item.Name.Replace("ExistTabel", "");
                        var restorItem = routes.FirstOrDefault(x => x.Name == $"{name}Restore");

                        rout = restorItem.GetValue(typeof(ApiRoutes.Recoveries)).ToString();

                        var answer = await _service.Call<bool>(token, rout, CallMethodType.Post, data, ("schema", schema), ("tableName", tableName));

                        return new JsonResult(answer);
                    }
                }
            }

            return new JsonResult(new Response<bool> { Data = false, Message = "خطا در انجام عملیات بازیابی" });
        }
    }
}
