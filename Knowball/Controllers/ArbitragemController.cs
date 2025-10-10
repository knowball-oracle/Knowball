using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArbitragemController : ControllerBase
    {
        private readonly IArbitragemService _arbitragemService;

        public ArbitragemController(IArbitragemService arbitragemService)
        {
            _arbitragemService = arbitragemService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArbitragemDto>> GetAll()
        {
            return Ok(_arbitragemService.ListarArbitragens());
        }

        [HttpGet("{idPartida}/{idArbitro}")]
        public ActionResult<ArbitragemDto> GetByIds(int idPartida, int idArbitro)
        {
            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
                return NotFound();

            return Ok(arbitragem);
        }

        [HttpPost]
        public ActionResult<ArbitragemDto> Create([FromBody] ArbitragemDto dto)
        {
            var created = _arbitragemService.CriarArbitragem(dto);
            return CreatedAtAction(nameof(GetByIds), new { idPartida = created.IdPartida, idArbitro = created.IdArbitro }, created);
        }
    }
}
