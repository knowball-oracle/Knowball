using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Authorize]
    [Route("api/arbitros")]
    [ApiController]
    public class ArbitroController : ControllerBase
    {
        private readonly IArbitroService _arbitroService;
        private readonly ILogger<ArbitroController> _logger;

        public ArbitroController(IArbitroService arbitroService, ILogger<ArbitroController> logger)
        {
            _arbitroService = arbitroService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArbitroDto>> GetAll()
        {
            var arbitros = _arbitroService.ListarArbitros();
            var response = new
            {
                data = arbitros,
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
        public ActionResult<ArbitroDto> GetById(int id)
        {
            var arbitro = _arbitroService.ObterPorId(id);
            if (arbitro == null)
            {
                _logger.LogWarning("Árbitro não encontrado: IdArbitro={IdArbitro}", id);
                return NotFound(new { message = "Árbitro não encontrado" });
            }

            var response = new
            {
                data = arbitro,
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ArbitroDto> Create([FromBody] ArbitroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdArbitro = _arbitroService.CriarArbitro(dto);

            var response = new
            {
                data = createdArbitro,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id = createdArbitro.IdArbitro }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = createdArbitro.IdArbitro }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = createdArbitro.IdArbitro }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };

            return CreatedAtAction(nameof(GetById), new { id = createdArbitro.IdArbitro }, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ArbitroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdArbitro != 0 && id != dto.IdArbitro)
            {
                _logger.LogWarning("ID incompatível na atualização de árbitro: rota={IdRota}, body={IdBody}", id, dto.IdArbitro);
                return BadRequest(new { message = "ID incompatível" });
            }

            var arbitro = _arbitroService.ObterPorId(id);
            if (arbitro == null)
            {
                _logger.LogWarning("Árbitro não encontrado para atualização: IdArbitro={IdArbitro}", id);
                return NotFound(new { message = "Árbitro não encontrado" });
            }

            _arbitroService.AtualizarArbitro(id, dto);

            var response = new
            {
                message = "Árbitro atualizado com sucesso",
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id }), method = "GET" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var arbitro = _arbitroService.ObterPorId(id);
            if (arbitro == null)
            {
                _logger.LogWarning("Árbitro não encontrado para remoção: IdArbitro={IdArbitro}", id);
                return NotFound(new { message = "Árbitro não encontrado" });
            }

            _arbitroService.RemoverArbitro(id);

            var response = new
            {
                message = "Árbitro removido com sucesso",
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
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] string? nome = null,
            [FromQuery] string? status = null,
            [FromQuery] DateTime? dataNascimentoInicio = null,
            [FromQuery] DateTime? dataNascimentoFim = null)
        {
            _logger.LogInformation("Busca de árbitros: Page={Page}, PageSize={PageSize}, Nome={Nome}, Status={Status}",
                page, pageSize, nome, status);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _arbitroService.ListarArbitros().AsQueryable();

            if (!string.IsNullOrEmpty(nome))
                query = query.Where(a => a.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            if (dataNascimentoInicio.HasValue)
                query = query.Where(a => a.DataNascimento.HasValue && a.DataNascimento >= dataNascimentoInicio);
            if (dataNascimentoFim.HasValue)
                query = query.Where(a => a.DataNascimento.HasValue && a.DataNascimento <= dataNascimentoFim);

            query = orderBy?.ToLower() switch
            {
                "nome" => query.OrderBy(a => a.Nome),
                "nome_desc" => query.OrderByDescending(a => a.Nome),
                "status" => query.OrderBy(a => a.Status),
                "datanascimento" => query.OrderBy(a => a.DataNascimento),
                "datanascimento_desc" => query.OrderByDescending(a => a.DataNascimento),
                _ => query.OrderBy(a => a.IdArbitro)
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
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, nome, status, dataNascimentoInicio, dataNascimentoFim }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, nome, status, dataNascimentoInicio, dataNascimentoFim }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, nome, status, dataNascimentoInicio, dataNascimentoFim }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, nome, status, dataNascimentoInicio, dataNascimentoFim }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, nome, status, dataNascimentoInicio, dataNascimentoFim }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };

            return Ok(response);
        }
    }
}