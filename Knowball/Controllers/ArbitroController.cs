using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArbitroController : ControllerBase
    {
        private readonly IArbitroService _arbitroService;

        public ArbitroController(IArbitroService arbitroService)
        {
            _arbitroService = arbitroService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArbitroDto>> GetAll()
        {
            var arbitros = _arbitroService.ListarArbitros();
            return Ok(arbitros);
        }

        [HttpGet("{id}")]
        public ActionResult<ArbitroDto> GetById(int id)
        {
            var arbitro = _arbitroService.ObterPorId(id);
            if (arbitro == null)
                return NotFound();

            return Ok(arbitro);
        }

        [HttpPost]
        public ActionResult<ArbitroDto> Create([FromBody] ArbitroDto dto)
        {
            var createdArbitro = _arbitroService.CriarArbitro(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdArbitro.IdArbitro }, createdArbitro);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ArbitroDto dto)
        {
            if (dto.IdArbitro != 0 && id != dto.IdArbitro)
                return BadRequest();

            _arbitroService.AtualizarArbitro(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _arbitroService.RemoverArbitro(id);
            return NoContent();
        }
    }
}
