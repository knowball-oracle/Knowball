using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasApiController : ControllerBase
    {
        private readonly IPartidaService _partidaService;

        public PartidasApiController(IPartidaService partidaService)
        {
            _partidaService = partidaService;
        }

        /// <summary>
        /// Retorna todas as partidas
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<PartidaDto>> GetAll()
        {
            var partidas = _partidaService.ListarPartidas();
            var response = new
            {
                data = partidas,
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
        /// Retorna uma partida pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<PartidaDto> GetById(int id)
        {
            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
                return NotFound(new { message = "Partida não encontrada" });

            var response = new
            {
                data = partida,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Cria uma nova partida
        /// </summary>
        [HttpPost]
        public ActionResult<PartidaDto> Create([FromBody] PartidaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPartida = _partidaService.CriarPartida(dto);
            var response = new
            {
                data = createdPartida,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id = createdPartida.IdPartida }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = createdPartida.IdPartida }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = createdPartida.IdPartida }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return CreatedAtAction(nameof(GetById), new { id = createdPartida.IdPartida }, response);
        }

        /// <summary>
        /// Atualiza uma partida existente
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PartidaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdPartida != 0 && id != dto.IdPartida)
                return BadRequest(new { message = "ID incompatível" });

            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
                return NotFound(new { message = "Partida não encontrada" });

            _partidaService.AtualizarPartida(id, dto);

            var response = new
            {
                message = "Partida atualizada com sucesso",
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id }), method = "GET" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Remove uma partida
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
                return NotFound(new { message = "Partida não encontrada" });

            _partidaService.RemoverPartida(id);

            var response = new
            {
                message = "Partida removida com sucesso",
                links = new[]
                {
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Busca partidas com paginação, ordenação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] int? idCampeonato = null,
            [FromQuery] int? idEquipe1 = null,
            [FromQuery] int? idEquipe2 = null,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null,
            [FromQuery] string? local = null,
            [FromQuery] int? placarMandanteMin = null,
            [FromQuery] int? placarVisitanteMin = null
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _partidaService.ListarPartidas().AsQueryable();

            // Aplicar filtros
            if (idCampeonato.HasValue)
                query = query.Where(p => p.IdCampeonato == idCampeonato.Value);

            if (idEquipe1.HasValue)
                query = query.Where(p => p.IdEquipe1 == idEquipe1.Value);

            if (idEquipe2.HasValue)
                query = query.Where(p => p.IdEquipe2 == idEquipe2.Value);

            if (dataInicio.HasValue)
                query = query.Where(p => p.DataPartida >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(p => p.DataPartida <= dataFim.Value);

            if (!string.IsNullOrEmpty(local))
                query = query.Where(p => p.Local.Contains(local, StringComparison.OrdinalIgnoreCase));

            if (placarMandanteMin.HasValue)
                query = query.Where(p => p.PlacarMandante >= placarMandanteMin.Value);

            if (placarVisitanteMin.HasValue)
                query = query.Where(p => p.PlacarVisitante >= placarVisitanteMin.Value);

            // Aplicar ordenação
            query = orderBy?.ToLower() switch
            {
                "data" => query.OrderBy(p => p.DataPartida),
                "data_desc" => query.OrderByDescending(p => p.DataPartida),
                "local" => query.OrderBy(p => p.Local),
                "local_desc" => query.OrderByDescending(p => p.Local),
                "placarmandante" => query.OrderBy(p => p.PlacarMandante),
                "placarmandante_desc" => query.OrderByDescending(p => p.PlacarMandante),
                "placarvisitante" => query.OrderBy(p => p.PlacarVisitante),
                "placarvisitante_desc" => query.OrderByDescending(p => p.PlacarVisitante),
                _ => query.OrderByDescending(p => p.DataPartida)
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
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, idCampeonato, idEquipe1, idEquipe2, dataInicio, dataFim, local, placarMandanteMin, placarVisitanteMin }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, idCampeonato, idEquipe1, idEquipe2, dataInicio, dataFim, local, placarMandanteMin, placarVisitanteMin }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, idCampeonato, idEquipe1, idEquipe2, dataInicio, dataFim, local, placarMandanteMin, placarVisitanteMin }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, idCampeonato, idEquipe1, idEquipe2, dataInicio, dataFim, local, placarMandanteMin, placarVisitanteMin }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, idCampeonato, idEquipe1, idEquipe2, dataInicio, dataFim, local, placarMandanteMin, placarVisitanteMin }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };

            return Ok(response);
        }
    }
}
