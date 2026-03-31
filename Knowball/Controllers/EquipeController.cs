using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeService _equipeService;
        private readonly ILogger<EquipeController> _logger;

        public EquipeController(IEquipeService equipeService, ILogger<EquipeController> logger)
        {
            _equipeService = equipeService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EquipeDto>> GetAll()
        {
            var equipes = _equipeService.ListarEquipes();
            var response = new
            {
                data = equipes,
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
        public ActionResult<EquipeDto> GetById(int id)
        {
            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
            {
                _logger.LogWarning("Equipe não encontrada: IdEquipe={IdEquipe}", id);
                return NotFound(new { message = "Equipe não encontrada" });
            }

            var response = new
            {
                data = equipe,
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
        public ActionResult<EquipeDto> Create([FromBody] EquipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdEquipe = _equipeService.CriarEquipe(dto);
                var response = new
                {
                    data = createdEquipe,
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), new { id = createdEquipe.IdEquipe }), method = "GET" },
                        new { rel = "update", href = Url.Action(nameof(Update), new { id = createdEquipe.IdEquipe }), method = "PUT" },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { id = createdEquipe.IdEquipe }), method = "DELETE" },
                        new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                    }
                };
                return CreatedAtAction(nameof(GetById), new { id = createdEquipe.IdEquipe }, response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar equipe: Nome={Nome}, Estado={Estado}", dto.Nome, dto.Estado);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EquipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdEquipe != 0 && id != dto.IdEquipe)
            {
                _logger.LogWarning("ID incompatível na atualização de equipe: rota={IdRota}, body={IdBody}", id, dto.IdEquipe);
                return BadRequest(new { message = "ID incompatível" });
            }

            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
            {
                _logger.LogWarning("Equipe não encontrada para atualização: IdEquipe={IdEquipe}", id);
                return NotFound(new { message = "Equipe não encontrada" });
            }

            try
            {
                _equipeService.AtualizarEquipe(id, dto);
                var response = new
                {
                    message = "Equipe atualizada com sucesso",
                    links = new[]
                    {
                        new { rel = "self", href = Url.Action(nameof(GetById), new { id }), method = "GET" },
                        new { rel = "delete", href = Url.Action(nameof(Delete), new { id }), method = "DELETE" },
                        new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                    }
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação ao atualizar equipe: IdEquipe={IdEquipe}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
            {
                _logger.LogWarning("Equipe não encontrada para remoção: IdEquipe={IdEquipe}", id);
                return NotFound(new { message = "Equipe não encontrada" });
            }

            _equipeService.RemoverEquipe(id);
            var response = new
            {
                message = "Equipe removida com sucesso",
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
            [FromQuery] string? cidade = null, [FromQuery] string? estado = null)
        {
            _logger.LogInformation("Busca de equipes: Page={Page}, PageSize={PageSize}, Nome={Nome}, Estado={Estado}",
                page, pageSize, nome, estado);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _equipeService.ListarEquipes().AsQueryable();

            if (!string.IsNullOrEmpty(nome)) query = query.Where(e => e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(cidade)) query = query.Where(e => e.Cidade != null && e.Cidade.Contains(cidade, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(estado)) query = query.Where(e => e.Estado != null && e.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));

            query = orderBy?.ToLower() switch
            {
                "nome" => query.OrderBy(e => e.Nome),
                "nome_desc" => query.OrderByDescending(e => e.Nome),
                "cidade" => query.OrderBy(e => e.Cidade),
                "cidade_desc" => query.OrderByDescending(e => e.Cidade),
                "estado" => query.OrderBy(e => e.Estado),
                "estado_desc" => query.OrderByDescending(e => e.Estado),
                _ => query.OrderBy(e => e.Nome)
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
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, nome, cidade, estado }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, nome, cidade, estado }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, nome, cidade, estado }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, nome, cidade, estado }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, nome, cidade, estado }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }
    }
}