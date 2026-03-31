using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipacaoController : ControllerBase
    {
        private readonly IParticipacaoService _participacaoService;
        private readonly ILogger<ParticipacaoController> _logger;

        public ParticipacaoController(IParticipacaoService participacaoService, ILogger<ParticipacaoController> logger)
        {
            _participacaoService = participacaoService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ParticipacaoDto>> GetAll()
        {
            var participacoes = _participacaoService.ListarParticipacoes();
            var response = new
            {
                data = participacoes,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }

        [HttpGet("{idPartida}/{idEquipe}")]
        public ActionResult<ParticipacaoDto> GetByIds(int idPartida, int idEquipe)
        {
            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
            {
                _logger.LogWarning("Participação não encontrada: PartidaId={IdPartida}, EquipeId={IdEquipe}", idPartida, idEquipe);
                return NotFound(new { message = "Participação não encontrada" });
            }

            var response = new
            {
                data = participacao,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida, idEquipe }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { idPartida, idEquipe }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida, idEquipe }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ParticipacaoDto> Create([FromBody] ParticipacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = _participacaoService.CriarParticipacao(dto);
                var response = new
                {
                    data = created,
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida = created.IdPartida, idEquipe = created.IdEquipe }), method = "GET" },
                        new { rel = "update", href = Url.Action(nameof(Update), new { idPartida = created.IdPartida, idEquipe = created.IdEquipe }), method = "PUT" },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida = created.IdPartida, idEquipe = created.IdEquipe }), method = "DELETE" },
                        new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                    }
                };
                return CreatedAtAction(nameof(GetByIds), new { idPartida = created.IdPartida, idEquipe = created.IdEquipe }, response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar participação: PartidaId={IdPartida}, EquipeId={IdEquipe}, Tipo={Tipo}",
                    dto.IdPartida, dto.IdEquipe, dto.Tipo);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{idPartida}/{idEquipe}")]
        public IActionResult Update(int idPartida, int idEquipe, [FromBody] ParticipacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPartida != dto.IdPartida || idEquipe != dto.IdEquipe)
            {
                _logger.LogWarning("IDs incompatíveis na atualização de participação: rota=({IdPartidaRota},{IdEquipeRota}), body=({IdPartidaBody},{IdEquipeBody})",
                    idPartida, idEquipe, dto.IdPartida, dto.IdEquipe);
                return BadRequest(new { message = "IDs incompatíveis" });
            }

            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
            {
                _logger.LogWarning("Participação não encontrada para atualização: PartidaId={IdPartida}, EquipeId={IdEquipe}", idPartida, idEquipe);
                return NotFound(new { message = "Participação não encontrada" });
            }

            try
            {
                _participacaoService.AtualizarParticipacao(idPartida, idEquipe, dto);
                var response = new
                {
                    message = "Participação atualizada com sucesso",
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida, idEquipe }), method = "GET" },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida, idEquipe }), method = "DELETE" },
                        new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                    }
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação ao atualizar participação: PartidaId={IdPartida}, EquipeId={IdEquipe}", idPartida, idEquipe);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{idPartida}/{idEquipe}")]
        public IActionResult Delete(int idPartida, int idEquipe)
        {
            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
            {
                _logger.LogWarning("Participação não encontrada para remoção: PartidaId={IdPartida}, EquipeId={IdEquipe}", idPartida, idEquipe);
                return NotFound(new { message = "Participação não encontrada" });
            }

            _participacaoService.RemoverParticipacao(idPartida, idEquipe);
            var response = new
            {
                message = "Participação removida com sucesso",
                links = new[]
                {
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }
    }
}