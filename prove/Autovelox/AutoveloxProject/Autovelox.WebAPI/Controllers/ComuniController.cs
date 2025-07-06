using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Autovelox.Data.Models;
using Autovelox.Application.Services;

namespace Autovelox.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComuniController : Controller
    {
        private readonly ComuniService _service;

        public ComuniController(ComuniService service)
        {
            _service = service;
        }

        [HttpGet("GetComuni")]
        public async Task<IActionResult> GetComuni()
        {
            var comuni = await _service.GetAllAsync();
            return Ok(comuni);
        }

        [HttpGet("GetComune/{idComune}")]
        public async Task<IActionResult> GetComune(int idComune)
        {
            var comune = await _service.GetById(idComune);
            return Ok(comune);
        }

        [HttpPost("GetComuni")]
        public async Task<IActionResult> GetComuni([FromBody]string? textFilter)
        {
            //var body = StreamReader(Request.Body);
            var comuni = await _service.GetByTextFilter(textFilter);
            return Ok(comuni);
        }
    }
}
