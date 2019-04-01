using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto;
using API.Interfaces;
using AutoMapper;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/texts")]
    public class TextController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITextService _service;

        public TextController(IMapper mapper, ITextService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTexts(Guid sourceId)
        {
            var texts = await _service.GetTexts(sourceId);

            var textsDto = _mapper.Map<List<TextDto>>(texts);

            return Ok(textsDto);
        }

        [HttpGet]
        [Route("{textId}")]
        public async Task<IActionResult> GetTextValue(Guid textId)
        {
            var textEntity = await _service.GetText(textId);
            var textDto = _mapper.Map<TextDto>(textEntity);

            return Ok(textDto);
        }

        [HttpPost]
        public async Task<IActionResult> SaveText(TextSaveDto text)
        {
            var textEntity = _mapper.Map<Text>(text);

            var id = await _service.SaveText(textEntity);

            return Ok(id);
        }
    }
}