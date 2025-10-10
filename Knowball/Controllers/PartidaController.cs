using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Application.DTOs
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly IPartidaService _partidaService;

        public PartidaController(IPartidaService partidaService)
        {
            _partidaService = partidaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PartidaDto>> GetAll()
        {
            var partidas = _partidaService.ListarPartidas();
            return Ok(partidas);
        }

        [HttpGet("{id}")]
        public ActionResult<PartidaDto> GetById(int id)
        {
            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
                return NotFound();

            return Ok(partida);
        }

        [HttpPost]
        public ActionResult<PartidaDto> Create([FromBody] PartidaDto dto)
        {
            var createdPartida = _partidaService.CriarPartida(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdPartida.IdPartida }, createdPartida);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PartidaDto dto)
        {
            if (dto.IdPartida != 0 && id != dto.IdPartida)
                return BadRequest();

            _partidaService.AtualizarPartida(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _partidaService.RemoverPartida(id);
            return NoContent();
        }
    }
}
