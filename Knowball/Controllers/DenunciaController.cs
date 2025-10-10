using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DenunciaController : ControllerBase
    {
        private readonly IDenunciaService _denunciaService;

        public DenunciaController(IDenunciaService denunciaService)
        {
            _denunciaService = denunciaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DenunciaDto>> GetAll()
        {
            var denuncias = _denunciaService.ListarDenuncias();
            return Ok(denuncias);
        }

        [HttpGet("{id}")]
        public ActionResult<DenunciaDto> GetById(int id)
        {
            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
                return NotFound();

            return Ok(denuncia);
        }

        [HttpPost]
        public ActionResult<DenunciaDto> Create([FromBody] DenunciaDto dto)
        {
            var createdDenuncia = _denunciaService.CriarDenuncia(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdDenuncia.IdDenuncia }, createdDenuncia);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DenunciaDto dto)
        {
            if (dto.IdDenuncia != 0 && id != dto.IdDenuncia)
                return BadRequest();

            _denunciaService.AtualizarDenuncia(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _denunciaService.RemoverDenuncia(id);
            return NoContent();
        }
    }
}
