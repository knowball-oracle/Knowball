using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Exceptions;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampeonatosApiController : ControllerBase
    {
        private readonly ICampeonatoService _campeonatoService;
        private readonly ILogger<CampeonatosApiController> _logger;

        public CampeonatosApiController(ICampeonatoService campeonatoService, ILogger<CampeonatosApiController> logger)
        {
            _campeonatoService = campeonatoService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CampeonatoDto>> GetAll()
        {
            var campeonatos = _campeonatoService.ListarCampeonatos();
            var response = new
            {
                data = campeonatos,
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
        public ActionResult<CampeonatoDto> GetById(int id)
        {
            var campeonato = _campeonatoService.ObterPorId(id);
            if (campeonato == null)
            {
                _logger.LogWarning("Campeonato não encontrado: IdCampeonato={IdCampeonato}", id);
                return NotFound(new { message = "Campeonato não encontrado" });
            }

            var response = new
            {
                data = campeonato,
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
        public ActionResult<CampeonatoDto> Create([FromBody] CampeonatoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCampeonato = _campeonatoService.CriarCampeonato(dto);
                var response = new
                {
                    data = createdCampeonato,
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), new { id = createdCampeonato.IdCampeonato }), method = "GET" },
                        new { rel = "update", href = Url.Action(nameof(Update), new { id = createdCampeonato.IdCampeonato }), method = "PUT" },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { id = createdCampeonato.IdCampeonato }), method = "DELETE" },
                        new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                    }
                };
                return CreatedAtAction(nameof(GetById), new { id = createdCampeonato.IdCampeonato }, response);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar campeonato: Nome={Nome}, Ano={Ano}, Categoria={Categoria}",
                    dto.Nome, dto.Ano, dto.Categoria);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CampeonatoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdCampeonato != 0 && id != dto.IdCampeonato)
            {
                _logger.LogWarning("ID incompatível na atualização de campeonato: rota={IdRota}, body={IdBody}", id, dto.IdCampeonato);
                return BadRequest(new { message = "ID incompatível" });
            }

            var campeonato = _campeonatoService.ObterPorId(id);
            if (campeonato == null)
            {
                _logger.LogWarning("Campeonato não encontrado para atualização: IdCampeonato={IdCampeonato}", id);
                return NotFound(new { message = "Campeonato não encontrado" });
            }

            try
            {
                _campeonatoService.AtualizarCampeonato(id, dto);
                var response = new
                {
                    message = "Campeonato atualizado com sucesso",
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
                _logger.LogError(ex, "Erro de validação ao atualizar campeonato: IdCampeonato={IdCampeonato}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var campeonato = _campeonatoService.ObterPorId(id);
            if (campeonato == null)
            {
                _logger.LogWarning("Campeonato não encontrado para remoção: IdCampeonato={IdCampeonato}", id);
                return NotFound(new { message = "Campeonato não encontrado" });
            }

            _campeonatoService.RemoverCampeonato(id);
            var response = new
            {
                message = "Campeonato removido com sucesso",
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
            [FromQuery] string? orderBy = null, [FromQuery] string? nome = null,
            [FromQuery] string? categoria = null, [FromQuery] int? ano = null,
            [FromQuery] int? anoInicio = null, [FromQuery] int? anoFim = null)
        {
            _logger.LogInformation("Busca de campeonatos: Page={Page}, PageSize={PageSize}, Nome={Nome}, Categoria={Categoria}, Ano={Ano}",
                page, pageSize, nome, categoria, ano);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _campeonatoService.ListarCampeonatos().AsQueryable();

            if (!string.IsNullOrEmpty(nome)) query = query.Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(categoria)) query = query.Where(c => c.Categoria.Contains(categoria, StringComparison.OrdinalIgnoreCase));
            if (ano.HasValue) query = query.Where(c => c.Ano == ano.Value);
            if (anoInicio.HasValue) query = query.Where(c => c.Ano >= anoInicio.Value);
            if (anoFim.HasValue) query = query.Where(c => c.Ano <= anoFim.Value);

            query = orderBy?.ToLower() switch
            {
                "nome" => query.OrderBy(c => c.Nome),
                "nome_desc" => query.OrderByDescending(c => c.Nome),
                "categoria" => query.OrderBy(c => c.Categoria),
                "categoria_desc" => query.OrderByDescending(c => c.Categoria),
                "ano" => query.OrderBy(c => c.Ano),
                "ano_desc" => query.OrderByDescending(c => c.Ano),
                _ => query.OrderByDescending(c => c.Ano).ThenBy(c => c.Nome)
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
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, nome, categoria, ano, anoInicio, anoFim }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, nome, categoria, ano, anoInicio, anoFim }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, nome, categoria, ano, anoInicio, anoFim }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, nome, categoria, ano, anoInicio, anoFim }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, nome, categoria, ano, anoInicio, anoFim }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }
    }
}