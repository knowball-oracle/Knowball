using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    [Route("[controller]")]
    public class ParticipacaoController : Controller
    {
        private readonly IParticipacaoService _participacaoService;
        private readonly IPartidaService _partidaService;
        private readonly IEquipeService _equipeService;
        private readonly ILogger<ParticipacaoController> _logger;

        public ParticipacaoController(IParticipacaoService participacaoService, IPartidaService partidaService,
            IEquipeService equipeService, ILogger<ParticipacaoController> logger)
        {
            _participacaoService = participacaoService ?? throw new ArgumentNullException(nameof(participacaoService));
            _partidaService = partidaService ?? throw new ArgumentNullException(nameof(partidaService));
            _equipeService = equipeService ?? throw new ArgumentNullException(nameof(equipeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(""), HttpGet("Index"), Route("")]
        public IActionResult Index()
        {
            try
            {
                var participacoes = _participacaoService.ListarParticipacoes();
                var viewModels = new List<ParticipacaoViewModel>();

                foreach (var p in participacoes)
                {
                    var partida = _partidaService.ObterPorId(p.IdPartida);
                    var equipe = _equipeService.ObterPorId(p.IdEquipe);

                    viewModels.Add(new ParticipacaoViewModel
                    {
                        IdPartida = p.IdPartida,
                        IdEquipe = p.IdEquipe,
                        Tipo = p.Tipo,
                        NomeEquipe = equipe?.Nome,
                        CidadeEquipe = equipe?.Cidade,
                        LocalPartida = partida?.Local,
                        DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm")
                    });
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar participações");
                TempData["Erro"] = "Erro ao listar participações";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("criar")]
        public IActionResult Create()
        {
            var partidas = _partidaService.ListarPartidas();
            ViewBag.Partidas = partidas.Select(p => new PartidaViewModel
            {
                IdPartida = p.IdPartida,
                Local = p.Local,
                DataPartida = p.DataPartida
            }).ToList();

            var equipes = _equipeService.ListarEquipes();
            ViewBag.Equipes = equipes.Select(e => new EquipeViewModel
            {
                IdEquipe = e.IdEquipe,
                Nome = e.Nome,
                Cidade = e.Cidade,
                Estado = e.Estado
            }).ToList();

            return View(new ParticipacaoViewModel());
        }

        [HttpPost("criar"), ValidateAntiForgeryToken]
        public IActionResult Create(ParticipacaoViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var partidas = _partidaService.ListarPartidas();
                    ViewBag.Partidas = partidas.Select(p => new PartidaViewModel
                    {
                        IdPartida = p.IdPartida,
                        Local = p.Local,
                        DataPartida = p.DataPartida
                    }).ToList();

                    var equipes = _equipeService.ListarEquipes();
                    ViewBag.Equipes = equipes.Select(e => new EquipeViewModel
                    {
                        IdEquipe = e.IdEquipe,
                        Nome = e.Nome,
                        Cidade = e.Cidade,
                        Estado = e.Estado
                    }).ToList();

                    return View(vm);
                }

                var dto = new ParticipacaoDto
                {
                    IdPartida = vm.IdPartida,
                    IdEquipe = vm.IdEquipe,
                    Tipo = vm.Tipo
                };
                _participacaoService.CriarParticipacao(dto);
                TempData["Sucesso"] = "Participação criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar participação");
                ModelState.AddModelError("", "Erro ao criar participação.");
                return View(vm);
            }
        }

        [HttpGet("deletar")]
        public IActionResult Delete(int idPartida, int idEquipe)
        {
            try
            {
                var participacao = _participacaoService.ObterPorIds(idPartida, idEquipe);
                if (participacao == null) return NotFound();

                var partida = _partidaService.ObterPorId(participacao.IdPartida);
                var equipe = _equipeService.ObterPorId(participacao.IdEquipe);

                var vm = new ParticipacaoViewModel
                {
                    IdPartida = participacao.IdPartida,
                    IdEquipe = participacao.IdEquipe,
                    Tipo = participacao.Tipo,
                    NomeEquipe = equipe?.Nome,
                    CidadeEquipe = equipe?.Cidade,
                    LocalPartida = partida?.Local,
                    DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm")
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter participação");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("deletar"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int idPartida, int idEquipe)
        {
            try
            {
                _participacaoService.RemoverParticipacao(idPartida, idEquipe);
                TempData["Sucesso"] = "Participação removida com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover participação");
                TempData["Erro"] = "Erro ao remover participação.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
