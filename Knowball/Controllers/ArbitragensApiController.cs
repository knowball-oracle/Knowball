using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArbitragensApiController : ControllerBase
    {
        private readonly IArbitragemService _arbitragemService;

        public ArbitragensApiController(IArbitragemService arbitragemService)
        {
            _arbitragemService = arbitragemService;
        }

        /// <summary>
        /// Retorna todas as arbitragens
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ArbitragemDto>> GetAll()
        {
            var arbitragens = _arbitragemService.ListarArbitragens();
            var response = new
            {
                data = arbitragens,
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
        /// Retorna uma arbitragem específica por IdPartida e IdArbitro
        /// </summary>
        [HttpGet("{idPartida}/{idArbitro}")]
        public ActionResult<ArbitragemDto> GetByIds(int idPartida, int idArbitro)
        {
            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
                return NotFound(new { message = "Arbitragem não encontrada" });

            var response = new
            {
                data = arbitragem,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida, idArbitro }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { idPartida, idArbitro }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida, idArbitro }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Cria uma nova arbitragem
        /// </summary>
        [HttpPost]
        public ActionResult<ArbitragemDto> Create([FromBody] ArbitragemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdArbitragem = _arbitragemService.CriarArbitragem(dto);
            var response = new
            {
                data = createdArbitragem,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida = createdArbitragem.IdPartida, idArbitro = createdArbitragem.IdArbitro }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { idPartida = createdArbitragem.IdPartida, idArbitro = createdArbitragem.IdArbitro }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida = createdArbitragem.IdPartida, idArbitro = createdArbitragem.IdArbitro }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return CreatedAtAction(nameof(GetByIds), new { idPartida = createdArbitragem.IdPartida, idArbitro = createdArbitragem.IdArbitro }, response);
        }

        /// <summary>
        /// Atualiza uma arbitragem existente
        /// </summary>
        [HttpPut("{idPartida}/{idArbitro}")]
        public IActionResult Update(int idPartida, int idArbitro, [FromBody] ArbitragemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPartida != dto.IdPartida || idArbitro != dto.IdArbitro)
                return BadRequest(new { message = "IDs incompatíveis" });

            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
                return NotFound(new { message = "Arbitragem não encontrada" });

            _arbitragemService.AtualizarArbitragem(idPartida, idArbitro, dto);

            var response = new
            {
                message = "Arbitragem atualizada com sucesso",
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetByIds), new { idPartida, idArbitro }), method = "GET" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { idPartida, idArbitro }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Remove uma arbitragem
        /// </summary>
        [HttpDelete("{idPartida}/{idArbitro}")]
        public IActionResult Delete(int idPartida, int idArbitro)
        {
            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
                return NotFound(new { message = "Arbitragem não encontrada" });

            _arbitragemService.RemoverArbitragem(idPartida, idArbitro);

            var response = new
            {
                message = "Arbitragem removida com sucesso",
                links = new[]
                {
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Busca arbitragens com paginação, ordenação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] int? idPartida = null,
            [FromQuery] int? idArbitro = null,
            [FromQuery] string? funcao = null
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _arbitragemService.ListarArbitragens().AsQueryable();

            // Aplicar filtros
            if (idPartida.HasValue)
                query = query.Where(a => a.IdPartida == idPartida.Value);

            if (idArbitro.HasValue)
                query = query.Where(a => a.IdArbitro == idArbitro.Value);

            if (!string.IsNullOrEmpty(funcao))
                query = query.Where(a => a.Funcao.Contains(funcao, StringComparison.OrdinalIgnoreCase));

            // Aplicar ordenação
            query = orderBy?.ToLower() switch
            {
                "funcao" => query.OrderBy(a => a.Funcao),
                "funcao_desc" => query.OrderByDescending(a => a.Funcao),
                "idpartida" => query.OrderBy(a => a.IdPartida),
                "idpartida_desc" => query.OrderByDescending(a => a.IdPartida),
                "idarbitro" => query.OrderBy(a => a.IdArbitro),
                "idarbitro_desc" => query.OrderByDescending(a => a.IdArbitro),
                _ => query.OrderBy(a => a.IdPartida).ThenBy(a => a.IdArbitro)
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
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, idPartida, idArbitro, funcao }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, idPartida, idArbitro, funcao }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, idPartida, idArbitro, funcao }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, idPartida, idArbitro, funcao }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, idPartida, idArbitro, funcao }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };

            return Ok(response);
        }
    }
}
