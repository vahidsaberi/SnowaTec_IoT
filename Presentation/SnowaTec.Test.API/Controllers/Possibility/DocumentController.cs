using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Helper;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Commands;
using SnowaTec.Test.Service.Features.Possibility.DocumentFeatures.Queries;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class DocumentController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        private readonly IMapper _mapper;

        private readonly IDateTimeService _dateTimeService;

        private readonly IPortalDbContext _portalContext;

        public DocumentController(IMapper mapper, IDateTimeService dateTimeService, IPortalDbContext portalContext)
        {
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _portalContext = portalContext;
        }

        [HttpGet(ApiRoutes.Documents.GetAll)]
        public async Task<IActionResult> GetAll(bool loadThumbnail = false)
        {
            return Ok(await Mediator.Send(new GetAllDocumentQuery { LoadThumbnail = loadThumbnail }));
        }

        [HttpGet(ApiRoutes.Documents.Get)]
        public async Task<IActionResult> GetById(long id, bool loadThumbnail = false)
        {
            return Ok(await Mediator.Send(new GetDocumentByIdQuery { Id = id, LoadThumbnail = loadThumbnail }));
        }

        [HttpPost(ApiRoutes.Documents.Create)]
        public async Task<IActionResult> Create(CreateDocumentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Documents.Update)]
        public async Task<IActionResult> Update(long id, UpdateDocumentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Documents.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteDocumentByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Documents.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteDocumentByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Documents.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query, bool loadThumbnail = false)
        {
            return Ok(await Mediator.Send(new FilterDocumentQuery { Query = query, LoadThumbnail = loadThumbnail }));
        }

        [HttpPost(ApiRoutes.Documents.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters, bool loadThumbnail = false)
        {
            return Ok(await Mediator.Send(new DataTableDocumentQuery { Parameters = parameters, LoadThumbnail = loadThumbnail }));
        }

        [HttpPost(ApiRoutes.Documents.Upload)]
        public async Task<IActionResult> UploadDocument([FromForm] IFormFile file)
        {
            if (file.Length > 0)
            {
                var fileContent = file.ContentType;
                var fileName = Path.GetFileName(file.FileName);
                var fileExtension = Path.GetExtension(fileName);

                var command = new CreateDocumentCommand()
                {
                    Name = fileName,
                    ContentType = fileContent,
                    Extension = fileExtension
                };

                using (var target = new MemoryStream())
                {
                    file.CopyTo(target);
                    command.Content = target.ToArray();
                }

                return Ok(await Mediator.Send(command));
            }

            return BadRequest(new Response<Document> { Data = null, Succeeded = false, Message = "فایلی برای دخیره وجود ندارد." });
        }

        [HttpPut(ApiRoutes.Documents.UpdateJustContent)]
        public async Task<IActionResult> UpdateDocument([FromRoute] long id, [FromBody] byte[] bytes)
        {
            try
            {
                var item = await Mediator.Send(new GetDocumentByIdQuery { Id = id });

                item.Data.Content = bytes;
                item.Data.UpdateRowDate = _dateTimeService.Now;

                var command = _mapper.Map<UpdateDocumentCommand>(item.Data);

                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<Document>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "خطا در انجام عملیات.",
                    Errors = new List<string> { ex.Message }
                }); ;
            }
        }

        [HttpGet(ApiRoutes.Documents.GetFileById)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(long id)
        {
            try
            {
                var item = await Mediator.Send(new GetDocumentByIdQuery { Id = id });

                if (item.Data is null)
                {
                    return NotFound(new Response<FileStreamResult>
                    {
                        Message = "File not found."
                    });
                }

                var bytes = item.Data.Content;
                //var image = ImageHelper.ByteArrayToImage(bytes);
                //if (image.Width > 2480)
                //{
                //    image = ImageHelper.ResizeImage(image, 2480, 3508);
                //    bytes = ImageHelper.ImageToByteArray(image, ImageFormat.Jpeg);
                //}

                var content = new MemoryStream(bytes);

                return File(content, FileHelper.MimeType(bytes));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<byte[]>
                {
                    Message = "خطا در انجام عملیات.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet(ApiRoutes.Documents.GetFileByKey)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(string key)
        {
            try
            {
                var item = await _portalContext.Documents.FirstOrDefaultAsync(x => x.Key == key);

                if (item == null)
                {
                    return NotFound(new Response<Document>
                    {
                        Message = "File not found."
                    });
                }

                var bytes = item.Content;

                var content = new MemoryStream(bytes);

                return File(content, FileHelper.MimeType(bytes));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<Document>
                {
                    Message = "خطا در انجام عملیات.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
