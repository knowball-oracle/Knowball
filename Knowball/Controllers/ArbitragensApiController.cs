using Fiap.Knowball.Application.DTOs;
using Fiap.Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Knowball.Controllers
{
    [Route("api/arbitragens")]
    [ApiController]
    public class ArbitragensApiController : ControllerBase
    {
        private readonly IArbitragemService _arbitragemService;
        private readonly ILogger<ArbitragensApiController> _logger;

        public ArbitragensApiController(IArbitragemService arbitragemService, ILogger<ArbitragensApiController> logger)
        {
            _arbitragemService = arbitragemService;
            _logger = logger;
        }

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

        [HttpGet("{idPartida}/{idArbitro}")]
        public ActionResult<ArbitragemDto> GetByIds(int idPartida, int idArbitro)
        {
            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
            {
                _logger.LogWarning("Arbitragem não encontrada: PartidaId={IdPartida}, ArbitroId={IdArbitro}", idPartida, idArbitro);
                return NotFound(new { message = "Arbitragem não encontrada" });
            }

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

        [HttpPost]
        public ActionResult<ArbitragemDto> Create([FromBody] ArbitragemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
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
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação ao criar arbitragem: PartidaId={IdPartida}, ArbitroId={IdArbitro}", dto.IdPartida, dto.IdArbitro);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{idPartida}/{idArbitro}")]
        public IActionResult Update(int idPartida, int idArbitro, [FromBody] ArbitragemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (idPartida != dto.IdPartida || idArbitro != dto.IdArbitro)
            {
                _logger.LogWarning("IDs incompatíveis na atualização de arbitragem: rota=({IdPartidaRota},{IdArbitroRota}), body=({IdPartidaBody},{IdArbitroBody})",
                    idPartida, idArbitro, dto.IdPartida, dto.IdArbitro);
                return BadRequest(new { message = "IDs incompatíveis" });
            }

            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
            {
                _logger.LogWarning("Arbitragem não encontrada para atualização: PartidaId={IdPartida}, ArbitroId={IdArbitro}", idPartida, idArbitro);
                return NotFound(new { message = "Arbitragem não encontrada" });
            }

            try
            {
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
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Erro de validação ao atualizar arbitragem: PartidaId={IdPartida}, ArbitroId={IdArbitro}", idPartida, idArbitro);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{idPartida}/{idArbitro}")]
        public IActionResult Delete(int idPartida, int idArbitro)
        {
            var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
            if (arbitragem == null)
            {
                _logger.LogWarning("Arbitragem não encontrada para remoção: PartidaId={IdPartida}, ArbitroId={IdArbitro}", idPartida, idArbitro);
                return NotFound(new { message = "Arbitragem não encontrada" });
            }

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

        [HttpGet("search")]
        public ActionResult<object> Search(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? orderBy = null, [FromQuery] int? idPartida = null,
            [FromQuery] int? idArbitro = null, [FromQuery] string? funcao = null)
        {
            _logger.LogInformation("Busca de arbitragens: Page={Page}, PageSize={PageSize}, IdPartida={IdPartida}, IdArbitro={IdArbitro}, Funcao={Funcao}",
                page, pageSize, idPartida, idArbitro, funcao);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _arbitragemService.ListarArbitragens().AsQueryable();

            if (idPartida.HasValue) query = query.Where(a => a.IdPartida == idPartida.Value);
            if (idArbitro.HasValue) query = query.Where(a => a.IdArbitro == idArbitro.Value);
            if (!string.IsNullOrEmpty(funcao)) query = query.Where(a => a.Funcao.Contains(funcao, StringComparison.OrdinalIgnoreCase));

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
            var results = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new
            {
                data = results,
                pagination = new { currentPage = page, pageSize, totalCount, totalPages },
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