using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/sources")]
    public class SourceController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISourceService _service;

        public SourceController(IMapper mapper, ISourceService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetSources()
        {
            var sources = await _service.GetSources();

            var sourcesDto = _mapper.Map<List<SourceDto>>(sources);

            return Ok(sourcesDto);
        }

        [HttpGet]
        [Route("{sourceId}/headers")]
        public async Task<IActionResult> GetHeaders(Guid sourceId)
        {
            var sourceHeaders = await _service.GetHeaders(sourceId);

            return Ok(sourceHeaders);
        }

        [HttpGet]
        [Route("{sourceId}/items")]
        public async Task<IActionResult> GetContent(Guid sourceId, int rows, int offset)
        {
            var sourceItems = await _service.GetContent(sourceId, rows, offset);

            return Ok(sourceItems);
        }

        [HttpGet]
        [Route("{sourceId}/phone-column")]
        public async Task<IActionResult> GetPhoneColumn(Guid sourceId)
        {
            var columnIndex = await _service.GetPhoneColumn(sourceId);

            return Ok(columnIndex);
        }

        [HttpPost]
        [Route("{sourceId}/phone-column")]
        public async Task<IActionResult> SetPhoneColumn(Guid sourceId, int columnIndex)
        {
            await _service.SetPhoneColumn(sourceId, columnIndex);

            return Ok();
        }

        [HttpPost]
        [Route("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile()
        {
            var name = Convert.ToString(Request.Form["name"]);

            var nameDuplicated = await _service.ValidateSourceNameNonDuplcate(name);

            if (nameDuplicated)
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            var file = Request.Form.Files["source"];

            var fileHasCorrectExtension = _service.ValidateSourceFileExtension(file);

            if (!fileHasCorrectExtension)
            {
                return new StatusCodeResult(StatusCodes.Status415UnsupportedMediaType);
            }

            var id = await _service.UploadSource(name, file);

            return Ok(id);
        }
    }
}
