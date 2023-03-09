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

namespace SnowaTec.Test.Web.Pages.Admin.Field
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

            var result = await _service.Call<DTResult<SnowaTec.Test.Domain.Entities.Possibility.Field>>(token, ApiRoutes.Fields.Pagination, CallMethodType.Post, dtModel);

            var model = _mapper.Map<List<FieldDto>>(result.Data.data);

            return new JsonResult(new DTResult<FieldDto>
            {
                data = model,
                draw = result.Data.draw,
                recordsFiltered = result.Data.recordsFiltered,
                recordsTotal = result.Data.recordsTotal
            });
        }

        public async Task<PartialViewResult> OnGetCreateUpdateAsync(long id)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var item = new FieldDto
            {
                Id = id,
            };

            if (id > 0)
            {
                var result = await _service.Call<SnowaTec.Test.Domain.Entities.Possibility.Field>(token, ApiRoutes.Fields.Get, CallMethodType.Get, null, ("id", id));
                item = _mapper.Map<FieldDto>(result.Data);
            }

            return LoadPartialView.Show<FieldDto>("_FieldEdit", item, ViewData, TempData);
        }

        public async Task<ActionResult> OnPostSaveAsync(FieldDto viewModel)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                Response<SnowaTec.Test.Domain.Entities.Possibility.Field> answer;

                if (!ModelState.IsValid)
                {
                    return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "اطلاعات وارد شده را بررسی کنید." });
                }

                viewModel.Title = viewModel.Title.Trim();
                viewModel.Key = viewModel.Key.Trim();

                if (viewModel.Id > 0)
                {
                    var Field = await _service.Call<SnowaTec.Test.Domain.Entities.Possibility.Field>(token, ApiRoutes.Fields.Get, CallMethodType.Get, null, ("id", viewModel.Id));

                    if (Field.Data.Title != viewModel.Title || Field.Data.Key != viewModel.Key)
                    {
                        Expression<Func<SnowaTec.Test.Domain.Entities.Possibility.Field, bool>> expected = x => x.Id != viewModel.Id && (x.Title == viewModel.Title || x.Key == viewModel.Key);
                        var find = await _service.Call<List<SnowaTec.Test.Domain.Entities.Possibility.Field>>(token, ApiRoutes.Fields.Filter, CallMethodType.Post, expected);

                        if (find.Data != null && find.Data.Count > 0)
                        {
                            if (find.Data.Any(x => x.Title == viewModel.Title))
                                return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "نام فیلد وارد شده قبلا ثبت شده است." });

                            if (find.Data.Any(x => x.Key == viewModel.Key))
                                return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "کلید فیلد وارد شده قبلا ثبت شده است." });
                        }
                    }

                    answer = await _service.Call<SnowaTec.Test.Domain.Entities.Possibility.Field>(token, ApiRoutes.Fields.Update, CallMethodType.Put, viewModel, ("id", viewModel.Id));
                }
                else
                {
                    Expression<Func<SnowaTec.Test.Domain.Entities.Possibility.Field, bool>> expected = x => x.Title == viewModel.Title || x.Key == viewModel.Key;
                    var find = await _service.Call<List<SnowaTec.Test.Domain.Entities.Possibility.Field>>(token, ApiRoutes.Fields.Filter, CallMethodType.Post, expected);

                    if (find.Data != null && find.Data.Count > 0)
                    {
                        if (find.Data.Any(x => x.Title == viewModel.Title))
                            return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "نام فیلد وارد شده قبلا ثبت شده است." });

                        if (find.Data.Any(x => x.Key == viewModel.Key))
                            return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "کلید فیلد وارد شده قبلا ثبت شده است." });
                    }

                    answer = await _service.Call<SnowaTec.Test.Domain.Entities.Possibility.Field>(token, ApiRoutes.Fields.Create, CallMethodType.Post, viewModel);
                }

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostDeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<SnowaTec.Test.Domain.Entities.Possibility.Field>(token, ApiRoutes.Fields.Delete, CallMethodType.Delete, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUndeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<SnowaTec.Test.Domain.Entities.Possibility.Field>(token, ApiRoutes.Fields.UnDelete, CallMethodType.Post, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<SnowaTec.Test.Domain.Entities.Possibility.Field> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }
        #endregion
    }
}
