using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampeonatoController : ControllerBase
    {
        private readonly ICampeonatoService _campeonatoService;

        public CampeonatoController(ICampeonatoService campeonatoService)
        {
            _campeonatoService = campeonatoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CampeonatoDto>> GetAll()
        {
            var campeonatos = _campeonatoService.ListarCampeonatos();
            return Ok(campeonatos);
        }

        [HttpGet("{id}")]
        public ActionResult<CampeonatoDto> GetById(int id)
        {
            var campeonato = _campeonatoService.ObterPorId(id);
            if (campeonato == null)
                return NotFound();

            return Ok(campeonato);
        }

        [HttpPost]
        public ActionResult<CampeonatoDto> Create([FromBody] CampeonatoDto dto)
        {
            var createdCampeonato = _campeonatoService.CriarCampeonato(dto);
            return CreatedAtAction(nameof(GetById), new { id =  createdCampeonato.IdCampeonato }, createdCampeonato);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CampeonatoDto dto)
        {
            if (dto.IdCampeonato != 0 && id != dto.IdCampeonato)
                return BadRequest();

            _campeonatoService.AtualizarCampeonato(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _campeonatoService.RemoverCampeonato(id);

            return NoContent();
        }
    }
}
