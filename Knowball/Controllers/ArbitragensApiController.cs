using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/arbitragens")] 
    public class ArbitragensApiController : ControllerBase 
    {
        private readonly IArbitragemService _arbitragemService;

        public ArbitragensApiController(IArbitragemService arbitragemService)
        {
            _arbitragemService = arbitragemService ?? throw new ArgumentNullException(nameof(arbitragemService));
        }

        /// <summary>
        /// Lista todas as arbitragens
        /// GET: api/arbitragens
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ArbitragemDto>> GetAll()
        {
            try
            {
                var arbitragens = _arbitragemService.ListarArbitragens();
                return Ok(arbitragens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao listar arbitragens", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém uma arbitragem específica por ID da Partida e ID do Árbitro
        /// GET: api/arbitragens/{idPartida}/{idArbitro}
        /// </summary>
        [HttpGet("{idPartida}/{idArbitro}")]
        public ActionResult<ArbitragemDto> GetByIds(int idPartida, int idArbitro)
        {
            try
            {
                var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
                if (arbitragem == null)
                    return NotFound(new { message = "Arbitragem não encontrada" });

                return Ok(arbitragem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao obter arbitragem", error = ex.Message });
            }
        }

        /// <summary>
        /// Cria uma nova arbitragem
        /// POST: api/arbitragens
        /// </summary>
        [HttpPost]
        public ActionResult<ArbitragemDto> Create([FromBody] ArbitragemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = _arbitragemService.CriarArbitragem(dto);
                return CreatedAtAction(
                    nameof(GetByIds),
                    new { idPartida = created.IdPartida, idArbitro = created.IdArbitro },
                    created
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao criar arbitragem", error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza uma arbitragem existente
        /// PUT: api/arbitragens/{idPartida}/{idArbitro}
        /// </summary>
        [HttpPut("{idPartida}/{idArbitro}")]
        public IActionResult Update(int idPartida, int idArbitro, [FromBody] ArbitragemDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (dto.IdPartida != idPartida || dto.IdArbitro != idArbitro)
                    return BadRequest(new { message = "IDs não correspondem" });

                _arbitragemService.AtualizarArbitragem(idPartida, idArbitro, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao atualizar arbitragem", error = ex.Message });
            }
        }

        /// <summary>
        /// Remove uma arbitragem
        /// DELETE: api/arbitragens/{idPartida}/{idArbitro}
        /// </summary>
        [HttpDelete("{idPartida}/{idArbitro}")]
        public IActionResult Delete(int idPartida, int idArbitro)
        {
            try
            {
                _arbitragemService.RemoverArbitragem(idPartida, idArbitro);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao remover arbitragem", error = ex.Message });
            }
        }
    }
}
