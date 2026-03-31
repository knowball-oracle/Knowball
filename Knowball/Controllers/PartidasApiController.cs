using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasApiController : ControllerBase
    {
        private readonly IPartidaService _partidaService;
        private readonly ILogger<PartidasApiController> _logger;

        public PartidasApiController(IPartidaService partidaService, ILogger<PartidasApiController> logger)
        {
            _partidaService = partidaService;
            _logger = logger;
        }

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

        [HttpGet("{id}")]
        public ActionResult<PartidaDto> GetById(int id)
        {
            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
            {
                _logger.LogWarning("Partida não encontrada: IdPartida={IdPartida}", id);
                return NotFound(new { message = "Partida não encontrada" });
            }

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

        [HttpPost]
        public ActionResult<PartidaDto> Create([FromBody] PartidaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
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
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar partida: CampeonatoId={IdCampeonato}, Data={DataPartida}",
                    dto.IdCampeonato, dto.DataPartida);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PartidaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdPartida != 0 && id != dto.IdPartida)
            {
                _logger.LogWarning("ID incompatível na atualização de partida: rota={IdRota}, body={IdBody}", id, dto.IdPartida);
                return BadRequest(new { message = "ID incompatível" });
            }

            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
            {
                _logger.LogWarning("Partida não encontrada para atualização: IdPartida={IdPartida}", id);
                return NotFound(new { message = "Partida não encontrada" });
            }

            try
            {
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
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Erro de validação ao atualizar partida: IdPartida={IdPartida}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var partida = _partidaService.ObterPorId(id);
            if (partida == null)
            {
                _logger.LogWarning("Partida não encontrada para remoção: IdPartida={IdPartida}", id);
                return NotFound(new { message = "Partida não encontrada" });
            }

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

        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null, [FromQuery] int? idCampeonato = null,
            [FromQuery] string? local = null, [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null)
        {
            _logger.LogInformation("Busca de partidas: Page={Page}, PageSize={PageSize}, CampeonatoId={IdCampeonato}, Local={Local}",
                page, pageSize, idCampeonato, local);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _partidaService.ListarPartidas().AsQueryable();

            if (idCampeonato.HasValue) query = query.Where(p => p.IdCampeonato == idCampeonato.Value);
            if (!string.IsNullOrEmpty(local)) query = query.Where(p => p.Local != null && p.Local.Contains(local, StringComparison.OrdinalIgnoreCase));
            if (dataInicio.HasValue) query = query.Where(p => p.DataPartida >= dataInicio.Value);
            if (dataFim.HasValue) query = query.Where(p => p.DataPartida <= dataFim.Value);

            query = orderBy?.ToLower() switch
            {
                "data" => query.OrderBy(p => p.DataPartida),
                "data_desc" => query.OrderByDescending(p => p.DataPartida),
                "local" => query.OrderBy(p => p.Local),
                "local_desc" => query.OrderByDescending(p => p.Local),
                "campeonato" => query.OrderBy(p => p.IdCampeonato),
                _ => query.OrderByDescending(p => p.DataPartida)
            };

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var results = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new
            {
                data = results,
                pagination = new { currentPage = page, pageSize, totalCount, totalPages },
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, idCampeonato, local, dataInicio, dataFim }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, idCampeonato, local, dataInicio, dataFim }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, idCampeonato, local, dataInicio, dataFim }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, idCampeonato, local, dataInicio, dataFim }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, idCampeonato, local, dataInicio, dataFim }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }
    }
}