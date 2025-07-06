using Autovelox.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autovelox.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MappeController : ControllerBase
    {
        private readonly MappeService service;

        public MappeController(MappeService service)
        {
            this.service = service;
        }

        [HttpGet("GetMappe")]
        public async Task<IActionResult> GetMappe()
        {
            var mappe = await service.GetAllAsync();
            return Ok(mappe);
        }

        [HttpGet("GetMappaById/{id}")]
        public async Task<IActionResult> GetMappaById(int id)
        {
            var mappa = await service.GetById(id);
            return Ok(mappa);
        }

        [HttpGet("GetMappeByComune/{idComune}")]
        public async Task<IActionResult> GetMappeByComune(int idComune)
        {
            var mappe = await service.GetByComune(idComune);
            return Ok(mappe);
        }

        [HttpGet("GetMappeByProvincia/{idProvincia}")]
        public async Task<IActionResult> GetMappeByProvincia(int idProvincia)
        {
            var mappe = await service.GetByProvincia(idProvincia);
            return Ok(mappe);
        }

        [HttpGet("GetMappeByRegione/{idRegione}")]
        public async Task<IActionResult> GetMappeByRegione(int idRegione)
        {
            var mappe = await service.GetByRegione(idRegione);
            return Ok(mappe);
        }

        [HttpGet("GetDettagliMappa/{id}")]
        public async Task<IActionResult> GetDettagliMappa(int id)
        {
            var mappa = await service.GetDettagliMappa(id);
            return Ok(mappa);
        }
    }
}
