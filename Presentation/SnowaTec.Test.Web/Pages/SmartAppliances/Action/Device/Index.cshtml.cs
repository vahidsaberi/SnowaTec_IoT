using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Dtos.Test;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.RealTime;
using SnowaTec.Test.Web.Enums;
using SnowaTec.Test.Web.Extensions;
using SnowaTec.Test.Web.Interfaces.Base;
using System.Linq.Expressions;

namespace SnowaTec.Test.Web.Pages.SmartAppliances.Action.Device
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

        [Inject]
        public MqttNetClient _mqttNetClient { get; }
        #endregion

        #region Constructor
        public IndexModel(
            ICallApiService service,
            IMapper mapper,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,
            IPermission permission,
            MqttNetClient mqttNetClient
        )
        {
            this._service = service;
            this._mapper = mapper;
            this._hostingEnvironment = environment;
            this._permission = permission;
            this._mqttNetClient = mqttNetClient;
            this._mqttNetClient.Initial(ReceiveMessage);
        }
        #endregion

        #region Methods
        public async Task<JsonResult> OnPostAsync(DTParameters dtModel)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var result = await _service.Call<DTResult<Domain.Entities.Test.Device>>(token, ApiRoutes.Devices.Pagination, CallMethodType.Post, dtModel);

            return new JsonResult(result.Data);
        }

        public async Task<PartialViewResult> OnGetCreateUpdateAsync(long id, long? parentId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var item = new DeviceDto
            {
                Id = id
            };

            if (id > 0)
            {
                var result = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Get, CallMethodType.Get, null, ("id", id));
                item = _mapper.Map<DeviceDto>(result.Data);
            }

            return LoadPartialView.Show<DeviceDto>("_DeviceEdit", item, ViewData, TempData);
        }

        public async Task<ActionResult> OnPostSaveAsync(DeviceDto viewModel)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                Response<Domain.Entities.Test.Device> answer;

                if (!ModelState.IsValid)
                {
                    return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "اطلاعات وارد شده را بررسی کنید." });
                }

                if (viewModel.Id > 0)
                {
                    var Device = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Get, CallMethodType.Get, null, ("id", viewModel.Id));

                    if (Device.Data.Name != viewModel.Name || Device.Data.ClientId != viewModel.ClientId)
                    {
                        Expression<Func<Domain.Entities.Test.Device, bool>> expected = x => x.Id != viewModel.Id && (x.ClientId == viewModel.ClientId || x.Topic == viewModel.Topic);
                        var find = await _service.Call<List<Domain.Entities.Test.Device>>(token, ApiRoutes.Devices.Filter, CallMethodType.Post, expected);

                        if (find.Data != null && find.Data.Count > 0)
                        {
                            if (find.Data.Any(x => x.ClientId == viewModel.ClientId))
                                return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "شناسه دستگاه وارد شده قبلا ثبت شده است." });

                            if (find.Data.Any(x => x.Topic == viewModel.Topic))
                                return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "کانال ارتباطی وارد شده قبلا ثبت شده است." });
                        }
                    }

                    answer = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Update, CallMethodType.Put, viewModel, ("id", viewModel.Id));
                }
                else
                {
                    Expression<Func<Domain.Entities.Test.Device, bool>> expected = x => x.ClientId == viewModel.ClientId || x.Topic == viewModel.Topic;
                    var find = await _service.Call<List<Domain.Entities.Test.Device>>(token, ApiRoutes.Devices.Filter, CallMethodType.Post, expected);

                    if (find.Data != null && find.Data.Count > 0)
                    {
                        if (find.Data.Any(x => x.ClientId == viewModel.ClientId))
                            return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "شناسه دستگاه وارد شده قبلا ثبت شده است." });

                        if (find.Data.Any(x => x.Topic == viewModel.Topic))
                            return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "کانال ارتباطی وارد شده قبلا ثبت شده است." });
                    }

                    answer = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Create, CallMethodType.Post, viewModel);
                }

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostDeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Delete, CallMethodType.Delete, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<ActionResult> OnPostUndeleteAsync(long id, int type)
        {
            try
            {
                var token = Request.Cookies["AccessToken"].ToString();

                var answer = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.UnDelete, CallMethodType.Post, null, ("id", id), ("type", type));

                return new JsonResult(answer);
            }
            catch (Exception ex)
            {
                return new JsonResult(new Response<Domain.Entities.Test.Device> { Message = "خطا در انجام عملیات.", Errors = new List<string> { ex.Message } });
            }
        }

        public async Task<IActionResult> OnPostImport(IFormFile file, long DeviceId)
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
                        headerCells.Add("Name", 0);
                        headerCells.Add("ClientId", 0);
                        headerCells.Add("Topic", 0);
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

                        var data = new List<Domain.Entities.Test.Device>();
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            var model = new Domain.Entities.Test.Device();
                            model.Name = row.GetCell(headerCells["Name"]).ToString();
                            model.ClientId = row.GetCell(headerCells["ClientId"]).ToString();
                            model.Topic = row.GetCell(headerCells["Topic"]).ToString();
                            model.Description = row.GetCell(headerCells["Description"]) != null ? row.GetCell(headerCells["Description"]).ToString() : "";

                            data.Add(model);
                        }

                        if (data.Count == 0) return new JsonResult(new Response<string> { Message = "Insert data failed." });

                        foreach (var item in data)
                        {
                            var result = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Create, CallMethodType.Post, item);
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
            var Response = await _service.Call<DTResult<Domain.Entities.Test.Device>>(token, ApiRoutes.Devices.Pagination, CallMethodType.Post, dtModel);

            if (!Response.Succeeded)
                return new JsonResult(new Response<string> { Message = "هیچ رکوردی برای ایجاد خروجی یافت نشد." });

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Devices.xlsx";
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
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("ClientId");
                row.CreateCell(3).SetCellValue("Topic");
                row.CreateCell(4).SetCellValue("Description");

                var index = 1;
                foreach (var item in Response.Data.data)
                {
                    row = excelSheet.CreateRow(index++);
                    row.CreateCell(0).SetCellValue(item.Id);
                    row.CreateCell(1).SetCellValue(item.Name);
                    row.CreateCell(2).SetCellValue(item.ClientId);
                    row.CreateCell(3).SetCellValue(item.Topic);
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

        //public async Task<PartialViewResult> OnGetImportView()
        //{
        //    var token = Request.Cookies["AccessToken"].ToString();

        //    Expression<Func<Domain.Entities.Test.Device, bool>> expected = x => x.Deleted == false;

        //    var employees = await _service.Call<List<Domain.Entities.Test.Device>>(token, ApiRoutes.Devices.Filter, CallMethodType.Post, expected);

        //    var EmployeeItems = new List<SelectListItem>();

        //    if (employees.Data.Count > 0)
        //    {
        //        EmployeeItems = employees.Data.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Title }).ToList();
        //    }

        //    return LoadPartialView.Show<List<SelectListItem>>("_Import", EmployeeItems, ViewData, TempData);
        //}

        public async Task<PartialViewResult> OnGetDeviceInfoAsync(long id, long? parentId)
        {
            var token = Request.Cookies["AccessToken"].ToString();

            var item = new DeviceDto
            {
                Id = id
            };

            if (id > 0)
            {
                var result = await _service.Call<Domain.Entities.Test.Device>(token, ApiRoutes.Devices.Get, CallMethodType.Get, null, ("id", id));
                item = _mapper.Map<DeviceDto>(result.Data);
            }

            return LoadPartialView.Show<DeviceDto>("_DeviceInfo", item, ViewData, TempData);
        }

        public async Task<JsonResult> OnGetConnectAsync(string clientId, string topic)
        {
            var messageConnect = await _mqttNetClient.Connect(clientId);

            var messageSubscribe = await _mqttNetClient.Subscribe(topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);

            return new JsonResult(new { messageConnect, messageSubscribe });
        }

        public async Task<JsonResult> OnGetDisconnectAsync(string clientId, string topic)
        {
            var messageConnect = await _mqttNetClient.Disconnect(clientId);

            return new JsonResult(new { messageConnect });
        }

        public async Task<JsonResult> OnGetTurnOnAsync(string clientId, string topic)
        {
            var message = await _mqttNetClient.Send(topic, "TurnOn", MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);

            return new JsonResult(new { message });
        }

        public async Task<JsonResult> OnGetTurnOffAsync(string clientId, string topic)
        {
            var message = await _mqttNetClient.Send(topic, "TurnOff", MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce);

            return new JsonResult(new { message });
        }

        public async Task ReceiveMessage(string message)
        {

        }
        #endregion
    }
}
