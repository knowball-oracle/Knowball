using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipacaoController : ControllerBase
    {
        private readonly IParticipacaoService _participacaoService;

        public ParticipacaoController(IParticipacaoService participacaoService)
        {
            _participacaoService = participacaoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ParticipacaoDto>> GetAll()
        {
            return Ok(_participacaoService.ListarParticipacoes());
        }

        [HttpGet("{idPartida}/{idEquipe}")]
        public ActionResult<ParticipacaoDto> GetByIds(int idPartida, int idEquipe)
        {
            var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
            if (participacao == null)
                return NotFound();

            return Ok(participacao);
        }

        [HttpPost]
        public ActionResult<ParticipacaoDto> Create([FromBody] ParticipacaoDto dto)
        {
            var created = _participacaoService.CriarParticipacao(dto);
            return CreatedAtAction(nameof(GetByIds), new { idPartida = created.IdPartida, idEquipe = created.IdEquipe }, created);
        }
    }
}
