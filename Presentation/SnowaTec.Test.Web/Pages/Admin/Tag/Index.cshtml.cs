using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.DTO.Possibility;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.Admin.Tag
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Properties
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }
        #endregion

        #region Constructor
        public IndexModel(ICallApiService service, IMapper mapper)
        {
            this._service = service;
            this._mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> OnPostAsync(DTParameters dtModel)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var query = string.Empty;

            if (!string.IsNullOrEmpty(dtModel.Columns[0].Search.Value))
            {
                var pid = dtModel.Columns[0].Search.Value;

                query += (query.Length > 0 ? " AND " : "") + (pid == "0" ? "parentId IS NULL" : $"parentId = {pid}");
            }
            else
            {
                query += (query.Length > 0 ? " AND " : "") + "parentId IS NULL";
            }

            dtModel.Query = query;

            var data = await _service.Call<DTResult<Domain.Entities.Possibility.Tag>>(token, ApiRoutes.Tags.Pagination, CallMethodType.Post, dtModel);

            return new JsonResult(new { draw = data.Data.draw, recordsTotal = data.Data.recordsTotal, recordsFiltered = data.Data.recordsFiltered, data = data.Data.data, error = "" });
        }

        public async Task<PartialViewResult> OnGetCreateUpdateAsync(long id, long? parentId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var item = new TagDto
            {
                Id = id,
                ParentId = parentId
            };

            if (id > 0)
            {
                var result = await _service.Call<Domain.Entities.Possibility.Tag>(Request.Cookies["AccessToken"].ToString(), ApiRoutes.Tags.Get, CallMethodType.Get, null, ("id", id));
                item = _mapper.Map<TagDto>(result.Data);
            }

            return LoadPartialView.Show<TagDto>("_TagEdit", item, ViewData, TempData);
        }

        public async Task<ActionResult> OnPostSaveAsync(TagDto viewModel)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                Response<Domain.Entities.Possibility.Tag> answer;

                if (!ModelState.IsValid)
                {
                    return new JsonResult(new Response<Domain.Entities.Possibility.Tag> { Message = "اطلاعات وارد شده را بررسی کنید." });
                }

                if (viewModel.Id > 0)
                {
                    var Tag = await _service.Call<Domain.Entities.Possibility.Tag>(token, ApiRoutes.Tags.Get, CallMethodType.Get, null, ("id", viewModel.Id));

                    if (Tag.Data.Title != viewModel.Title)
                    {
                        Expression<Func<Domain.Entities.Possibility.Tag, bool>> expected = x => x.Id != viewModel.Id && x.Title == viewModel.Title;
                        var find = await _service.Call<List<Domain.Entities.Possibility.Tag>>(token, ApiRoutes.Tags.Filter, CallMethodType.Post, expected);

                        if (find.Data != null && find.Data.Count > 0)
                        {
                            if (find.Data.Any(x => x.Title == viewModel.Title))
                                return new JsonResult(new Response<Domain.Entities.Possibility.Tag> { Message = "تگ وارد شده قبلا ثبت شده است." });
                        }
                    }

                    answer = await _service.Call<Domain.Entities.Possibility.Tag>(token, ApiRoutes.Tags.Update, CallMethodType.Put, viewModel, ("id", viewModel.Id));
                }
                else
                {
                    Expression<Func<Domain.Entities.Possibility.Tag, bool>> expected = x => x.Title == viewModel.Title;
                    var find = await _service.Call<List<Domain.Entities.Possibility.Tag>>(token, ApiRoutes.Tags.Filter, CallMethodType.Post, expected);

                    if (find.Data != null && find.Data.Count > 0)
                    {
                        if (find.Data.Any(x => x.Title == viewModel.Title))
                            return new JsonResult(new Response<Domain.Entities.Possibility.Tag> { Message = "تگ وارد شده قبلا ثبت شده است." });
                    }

                    answer = await _service.Call<Domain.Entities.Possibility.Tag>(token, ApiRoutes.Tags.Create, CallMethodType.Post, viewModel);
                }

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Possibility.Tag> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostDeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Domain.Entities.Possibility.Tag>(token, ApiRoutes.Tags.Delete, CallMethodType.Delete, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Possibility.Tag> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUndeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Domain.Entities.Possibility.Tag>(token, ApiRoutes.Tags.UnDelete, CallMethodType.Post, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Possibility.Tag> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }
        #endregion
    }
}
