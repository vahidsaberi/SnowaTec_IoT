using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Dtos.Test;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.SmartAppliances.Setting.Section
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Properties
        [Inject]
        public ICallApiService _service { get; }

        [Inject]
        private IMapper _mapper { get; }

        [Inject]
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment { get; }

        [Inject]
        public IPermission _permission { get; }
        #endregion

        #region Constructor
        public IndexModel(
            ICallApiService service,
            IMapper mapper,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,
            IPermission permission
        )
        {
            this._service = service;
            this._mapper = mapper;
            this._hostingEnvironment = environment;
            this._permission = permission;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> OnPostAsync(DTParameters dtModel)
        {

            var token = Request.Cookies["AccessToken"].ToString();

            var query = string.Empty;

            var subPermission = await _permission.GetSubPermissions(SubPermissionType.Test_Sections.ToString());
            if (subPermission.Count > 0)
            {
                query = $"(Id IN ({string.Join(",", subPermission)}))";
            }

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

            var result = await _service.Call<DTResult<Domain.Entities.Test.Section>>(token, ApiRoutes.Sections.Pagination, CallMethodType.Post, dtModel);

            return new JsonResult(result.Data);
        }

        public async Task<PartialViewResult> OnGetCreateUpdateAsync(long id, long? parentId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var item = new SectionDto
            {
                Id = id,
                ParentId = parentId
            };

            if (id > 0)
            {
                var result = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.Get, CallMethodType.Get, null, ("id", id));
                item = _mapper.Map<SectionDto>(result.Data);
            }

            return LoadPartialView.Show<SectionDto>("_SectionEdit", item, ViewData, TempData);
        }

        public async Task<ActionResult> OnPostSaveAsync(SectionDto viewModel)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                Response<Domain.Entities.Test.Section> answer;

                if (!ModelState.IsValid)
                {
                    return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "اطلاعات وارد شده را بررسی کنید." });
                }

                if (viewModel.Id > 0)
                {
                    var section = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.Get, CallMethodType.Get, null, ("id", viewModel.Id));

                    if (section.Data.Title != viewModel.Title || section.Data.Code != viewModel.Code)
                    {
                        Expression<Func<Domain.Entities.Test.Section, bool>> expected = x => x.Id != viewModel.Id && x.ParentId == viewModel.ParentId && (x.Title == viewModel.Title || x.Code == viewModel.Code);
                        var find = await _service.Call<List<Domain.Entities.Test.Section>>(token, ApiRoutes.Sections.Filter, CallMethodType.Post, expected);

                        if (find.Data != null && find.Data.Count > 0)
                        {
                            if (find.Data.Any(x => x.Title == viewModel.Title))
                                return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "نام فضا وارد شده قبلا ثبت شده است." });

                            if (find.Data.Any(x => x.Code == viewModel.Code))
                                return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "کد فضا وارد شده قبلا ثبت شده است." });
                        }
                    }

                    answer = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.Update, CallMethodType.Put, viewModel, ("id", viewModel.Id));
                }
                else
                {
                    Expression<Func<Domain.Entities.Test.Section, bool>> expected = x => x.ParentId == viewModel.ParentId && (x.Title == viewModel.Title || x.Code == viewModel.Code);
                    var find = await _service.Call<List<Domain.Entities.Test.Section>>(token, ApiRoutes.Sections.Filter, CallMethodType.Post, expected);

                    if (find.Data != null && find.Data.Count > 0)
                    {
                        if (find.Data.Any(x => x.Title == viewModel.Title))
                            return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "نام فضا وارد شده قبلا ثبت شده است." });

                        if (find.Data.Any(x => x.Code == viewModel.Code))
                            return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "کد فضا وارد شده قبلا ثبت شده است." });
                    }

                    answer = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.Create, CallMethodType.Post, viewModel);
                }

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostDeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.Delete, CallMethodType.Delete, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUndeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.UnDelete, CallMethodType.Post, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Test.Section> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<IActionResult> OnPostImport(IFormFile file, long sectionId)
        {
            try
            {
                if (file == null)
                {
                    return null;
                }

                var token = Request.Cookies["AccessToken"].ToString();
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        IRow headerRow = sheet.GetRow(0); //Get Header Row

                        var headerCells = new Dictionary<string, int>();
                        headerCells.Add("Code", 0);
                        headerCells.Add("Title", 0);
                        headerCells.Add("Description", 0);

                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;

                            if (!headerCells.Keys.Contains(cell.ToString()))
                            {
                                return new JsonResult(new Response<string> { Message = "Error in the sent file." });
                            }
                            else
                            {
                                headerCells[cell.ToString()] = j;
                            }
                        }

                        var data = new List<Domain.Entities.Test.Section>();
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            var model = new Domain.Entities.Test.Section();
                            model.ParentId = sectionId;
                            model.Code = row.GetCell(headerCells["Code"]).ToString();
                            model.Title = row.GetCell(headerCells["Title"]).ToString();
                            model.Description = row.GetCell(headerCells["Description"]) != null ? row.GetCell(headerCells["Description"]).ToString() : "";

                            data.Add(model);
                        }

                        if (data.Count == 0) return new JsonResult(new Response<string> { Message = "Insert data failed." });

                        foreach (var item in data)
                        {
                            var result = await _service.Call<Domain.Entities.Test.Section>(token, ApiRoutes.Sections.Create, CallMethodType.Post, item);
                        }
                    }

                    if (System.IO.File.Exists($"{newPath}\\{file.FileName}"))
                    {
                        System.IO.File.Delete($"{newPath}\\{file.FileName}");
                    }
                }

                return new JsonResult(new Response<string>("", "Registration information completed successfully."));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<string> { Succeeded = false, Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<IActionResult> OnPostExport(DTParameters dtModel)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            dtModel.Length = int.MaxValue;
            var Response = await _service.Call<DTResult<Domain.Entities.Test.Section>>(token, ApiRoutes.Sections.Pagination, CallMethodType.Post, dtModel);

            if (!Response.Succeeded)
                return new JsonResult(new Response<string> { Message = "هیچ رکوردی برای ایجاد خروجی یافت نشد." });

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Sections.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Data");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Id");
                row.CreateCell(1).SetCellValue("ParentId");
                row.CreateCell(2).SetCellValue("Code");
                row.CreateCell(3).SetCellValue("Title");
                row.CreateCell(4).SetCellValue("Description");

                var index = 1;
                foreach (var item in Response.Data.data)
                {
                    row = excelSheet.CreateRow(index++);
                    row.CreateCell(0).SetCellValue(item.Id);
                    row.CreateCell(1).SetCellValue(item.ParentId ?? 0);
                    row.CreateCell(2).SetCellValue(item.Code);
                    row.CreateCell(3).SetCellValue(item.Title);
                    row.CreateCell(4).SetCellValue(item.Description);
                }
                workbook.Write(fs, true);
            }

            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public async Task<PartialViewResult> OnGetImportView()
        {
            var token = Request.Cookies["AccessToken"].ToString();

            Expression<Func<Domain.Entities.Test.Section, bool>> expected = x => x.Deleted == false;

            var employees = await _service.Call<List<Domain.Entities.Test.Section>>(token, ApiRoutes.Sections.Filter, CallMethodType.Post, expected);

            var EmployeeItems = new List<SelectListItem>();

            if (employees.Data.Count > 0)
            {
                EmployeeItems = employees.Data.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Title }).ToList();
            }

            return LoadPartialView.Show<List<SelectListItem>>("_Import", EmployeeItems, ViewData, TempData);
        }
        #endregion
    }
}
