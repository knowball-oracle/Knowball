using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DenunciasApiController : ControllerBase
    {
        private readonly IDenunciaService _denunciaService;

        public DenunciasApiController(IDenunciaService denunciaService)
        {
            _denunciaService = denunciaService;
        }

        /// <summary>
        /// Retorna todas as denúncias
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<DenunciaDto>> GetAll()
        {
            var denuncias = _denunciaService.ListarDenuncias();
            var response = new
            {
                data = denuncias,
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
        /// Retorna uma denúncia pelo ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<DenunciaDto> GetById(int id)
        {
            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
                return NotFound(new { message = "Denúncia não encontrada" });

            var response = new
            {
                data = denuncia,
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
        /// Cria uma nova denúncia
        /// </summary>
        [HttpPost]
        public ActionResult<DenunciaDto> Create([FromBody] DenunciaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDenuncia = _denunciaService.CriarDenuncia(dto);
            var response = new
            {
                data = createdDenuncia,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id = createdDenuncia.IdDenuncia }), method = "GET" },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = createdDenuncia.IdDenuncia }), method = "PUT" },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = createdDenuncia.IdDenuncia }), method = "DELETE" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" }
                }
            };
            return CreatedAtAction(nameof(GetById), new { id = createdDenuncia.IdDenuncia }, response);
        }

        /// <summary>
        /// Atualiza uma denúncia existente
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] DenunciaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.IdDenuncia != 0 && id != dto.IdDenuncia)
                return BadRequest(new { message = "ID incompatível" });

            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
                return NotFound(new { message = "Denúncia não encontrada" });

            _denunciaService.AtualizarDenuncia(id, dto);

            var response = new
            {
                message = "Denúncia atualizada com sucesso",
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
        /// Remove uma denúncia
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
                return NotFound(new { message = "Denúncia não encontrada" });

            _denunciaService.RemoverDenuncia(id);

            var response = new
            {
                message = "Denúncia removida com sucesso",
                links = new[]
                {
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };
            return Ok(response);
        }

        /// <summary>
        /// Busca denúncias com paginação, ordenação e filtros
        /// </summary>
        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null,
            [FromQuery] string? protocolo = null,
            [FromQuery] string? status = null,
            [FromQuery] int? idPartida = null,
            [FromQuery] int? idArbitro = null,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null,
            [FromQuery] string? relato = null
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _denunciaService.ListarDenuncias().AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(protocolo))
                query = query.Where(d => d.Protocolo.Contains(protocolo, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(d => d.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            if (idPartida.HasValue)
                query = query.Where(d => d.IdPartida == idPartida.Value);

            if (idArbitro.HasValue)
                query = query.Where(d => d.IdArbitro == idArbitro.Value);

            if (dataInicio.HasValue)
                query = query.Where(d => d.DataDenuncia >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(d => d.DataDenuncia <= dataFim.Value);

            if (!string.IsNullOrEmpty(relato))
                query = query.Where(d => d.Relato.Contains(relato, StringComparison.OrdinalIgnoreCase));

            // Aplicar ordenação
            query = orderBy?.ToLower() switch
            {
                "protocolo" => query.OrderBy(d => d.Protocolo),
                "protocolo_desc" => query.OrderByDescending(d => d.Protocolo),
                "status" => query.OrderBy(d => d.Status),
                "status_desc" => query.OrderByDescending(d => d.Status),
                "data" => query.OrderBy(d => d.DataDenuncia),
                "data_desc" => query.OrderByDescending(d => d.DataDenuncia),
                _ => query.OrderByDescending(d => d.IdDenuncia)
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
                    new { rel = "self", href = Url.Action(nameof(Search), new { page, pageSize, orderBy, protocolo, status, idPartida, idArbitro, dataInicio, dataFim, relato }), method = "GET" },
                    new { rel = "first", href = Url.Action(nameof(Search), new { page = 1, pageSize, orderBy, protocolo, status, idPartida, idArbitro, dataInicio, dataFim, relato }), method = "GET" },
                    new { rel = "last", href = Url.Action(nameof(Search), new { page = totalPages, pageSize, orderBy, protocolo, status, idPartida, idArbitro, dataInicio, dataFim, relato }), method = "GET" },
                    new { rel = "next", href = page < totalPages ? Url.Action(nameof(Search), new { page = page + 1, pageSize, orderBy, protocolo, status, idPartida, idArbitro, dataInicio, dataFim, relato }) : null, method = "GET" },
                    new { rel = "previous", href = page > 1 ? Url.Action(nameof(Search), new { page = page - 1, pageSize, orderBy, protocolo, status, idPartida, idArbitro, dataInicio, dataFim, relato }) : null, method = "GET" },
                    new { rel = "all", href = Url.Action(nameof(GetAll)), method = "GET" },
                    new { rel = "create", href = Url.Action(nameof(Create)), method = "POST" }
                }
            };

            return Ok(response);
        }
    }
}
