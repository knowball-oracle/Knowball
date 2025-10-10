using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeService _equipeService;

        public EquipeController(IEquipeService equipeService)
        {
            _equipeService = equipeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EquipeDto>> GetAll()
        {
            return Ok(_equipeService.ListarEquipes());
        }

        [HttpGet("{id}")]
        public ActionResult<EquipeDto> GetById(int id)
        {
            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
                return NotFound();

            return Ok(equipe);
        }

        [HttpPost]
        public ActionResult<EquipeDto> Create([FromBody] EquipeDto dto)
        {
            var createdEquipe = _equipeService.CriarEquipe(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdEquipe.IdEquipe }, createdEquipe);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EquipeDto dto)
        {
            if (dto.IdEquipe != 0 && id != dto.IdEquipe)
                return BadRequest();

            _equipeService.AtualizarEquipe(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _equipeService.RemoverEquipe(id);
            return NoContent();
        }
    }
}
