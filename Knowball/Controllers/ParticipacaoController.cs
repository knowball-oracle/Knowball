using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipacaoController : ControllerBase
    {
        private readonly IParticipacaoService _participacaoService;

        public ParticipacaoController(IParticipacaoService participacaoService)
        {
            _participacaoService = participacaoService;
        }

        /// <summary>
        /// Retorna todas as participações
        /// </summary>
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
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" },
                    new { rel = "search", href = Url.Action(nameof(Search)), method = "GET" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Retorna uma participação específica por IdPartida e IdEquipe
        /// </summary>
        [HttpGet("{idPartida}/{idEquipe}")]
        public ActionResult<ParticipacaoDto> GetByIds(int idPartida, int idEquipe)
        {
            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
                return NotFound(new { message = "Participação não encontrada" });

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

        /// <summary>
        /// Cria uma nova participação
        /// </summary>
        [HttpPost]
        public ActionResult<ParticipacaoDto> Create([FromBody] ParticipacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdParticipacao = _participacaoService.CriarParticipacao(dto);
            var response = new
            {
                data = createdParticipacao,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida = createdParticipacao.IdPartida, idEquipe = createdParticipacao.IdEquipe }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { idPartida = createdParticipacao.IdPartida, idEquipe = createdParticipacao.IdEquipe }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida = createdParticipacao.IdPartida, idEquipe = createdParticipacao.IdEquipe }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return CreatedAtAction(nameof(GetByIds), new { idPartida = createdParticipacao.IdPartida, idEquipe = createdParticipacao.IdEquipe }, response);
        }

        /// <summary>
        /// Atualiza uma participação existente
        /// </summary>
        [HttpPut("{idPartida}/{idEquipe}")]
        public IActionResult Update(int idPartida, int idEquipe, [FromBody] ParticipacaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPartida != dto.IdPartida || idEquipe != dto.IdEquipe)
                return BadRequest(new { message = "IDs incompatíveis" });

            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
                return NotFound(new { message = "Participação não encontrada" });

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

        /// <summary>
        /// Remove uma participação
        /// </summary>
        [HttpDelete("{idPartida}/{idEquipe}")]
        public IActionResult Delete(int idPartida, int idEquipe)
        {
            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
                return NotFound(new { message = "Participação não encontrada" });

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

        /// <summary>
        /// Busca participações com paginação, ordenação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] int? idPartida = null,
            [FromQuery] int? idEquipe = null,
            [FromQuery] string? tipo = null
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _participacaoService.ListarParticipacoes().AsQueryable();

            // Aplicar filtros
            if (idPartida.HasValue)
                query = query.Where(p => p.IdPartida == idPartida.Value);

            if (idEquipe.HasValue)
                query = query.Where(p => p.IdEquipe == idEquipe.Value);

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(p => p.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));

            // Aplicar ordenação
            query = orderBy?.ToLower() switch
            {
                "tipo" => query.OrderBy(p => p.Tipo),
                "tipo_desc" => query.OrderByDescending(p => p.Tipo),
                "idpartida" => query.OrderBy(p => p.IdPartida),
                "idpartida_desc" => query.OrderByDescending(p => p.IdPartida),
                "idequipe" => query.OrderBy(p => p.IdEquipe),
                "idequipe_desc" => query.OrderByDescending(p => p.IdEquipe),
                _ => query.OrderBy(p => p.IdPartida).ThenBy(p => p.IdEquipe)
            };

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var results = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                data = results,
                pagination = new
                {
                    currentPage = page,
                    pageSize,
                    totalCount,
                    totalPages
                },
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, idPartida, idEquipe, tipo }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, idPartida, idEquipe, tipo }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, idPartida, idEquipe, tipo }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, idPartida, idEquipe, tipo }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, idPartida, idEquipe, tipo }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };

            return Ok(response);
        }
    }
}
