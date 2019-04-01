using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/events")]
    public class EventController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventController(IMapper mapper, IEventService eventService)
        {
            _mapper = mapper;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetEvents();

            var eventDtos = _mapper.Map<List<EventDto>>(events);

            return Ok(eventDtos);
        }

        [HttpPost]
        public async Task<IActionResult> NewEvent(Guid sourceId, Guid textId)
        {
            await _eventService.CreateEvent(sourceId, textId);
            return Ok();
        }
    }
}