using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeService _equipeService;

        public EquipeController(IEquipeService equipeService)
        {
            _equipeService = equipeService;
        }

        /// <summary>
        /// Retorna todas as equipes
        /// </summary>
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

        /// <summary>
        /// Retorna uma equipe pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<EquipeDto> GetById(int id)
        {
            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
                return NotFound(new { message = "Equipe não encontrada" });

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

        /// <summary>
        /// Cria uma nova equipe
        /// </summary>
        [HttpPost]
        public ActionResult<EquipeDto> Create([FromBody] EquipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

        /// <summary>
        /// Atualiza uma equipe existente
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] EquipeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdEquipe != 0 && id != dto.IdEquipe)
                return BadRequest(new { message = "ID incompatível" });

            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
                return NotFound(new { message = "Equipe não encontrada" });

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

        /// <summary>
        /// Remove uma equipe
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var equipe = _equipeService.ObterPorId(id);
            if (equipe == null)
                return NotFound(new { message = "Equipe não encontrada" });

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

        /// <summary>
        /// Busca equipes com paginação, ordenação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] string? nome = null,
            [FromQuery] string? cidade = null,
            [FromQuery] string? estado = null
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _equipeService.ListarEquipes().AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(e => e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(cidade))
                query = query.Where(e => e.Cidade.Contains(cidade, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(e => e.Estado.Equals(estado, StringComparison.OrdinalIgnoreCase));

            // Aplicar ordenação
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
