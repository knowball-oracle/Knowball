using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    [Route("[controller]")]
    public class ArbitragemController : Controller
    {
        private readonly IArbitragemService _arbitragemService;
        private readonly IPartidaService _partidaService;
        private readonly IArbitroService _arbitroService;
        private readonly ILogger<ArbitragemController> _logger;

        public ArbitragemController(IArbitragemService arbitragemService, IPartidaService partidaService,
            IArbitroService arbitroService, ILogger<ArbitragemController> logger)
        {
            _arbitragemService = arbitragemService ?? throw new ArgumentNullException(nameof(arbitragemService));
            _partidaService = partidaService ?? throw new ArgumentNullException(nameof(partidaService));
            _arbitroService = arbitroService ?? throw new ArgumentNullException(nameof(arbitroService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(""), HttpGet("Index"), Route("")]
        public IActionResult Index()
        {
            try
            {
                var arbitragens = _arbitragemService.ListarArbitragens();
                var viewModels = new List<ArbitragemViewModel>();

                foreach (var a in arbitragens)
                {
                    var partida = _partidaService.ObterPorId(a.IdPartida);
                    var arbitro = _arbitroService.ObterPorId(a.IdArbitro);

                    viewModels.Add(new ArbitragemViewModel
                    {
                        IdPartida = a.IdPartida,
                        IdArbitro = a.IdArbitro,
                        Funcao = a.Funcao,
                        NomeArbitro = arbitro?.Nome,
                        LocalPartida = partida?.Local,
                        DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm")
                    });
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar arbitragens");
                TempData["Erro"] = "Erro ao listar arbitragens";
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

            var arbitros = _arbitroService.ListarArbitros();
            ViewBag.Arbitros = arbitros.Select(a => new ArbitroViewModel
            {
                IdArbitro = a.IdArbitro,
                Nome = a.Nome
            }).ToList();

            return View(new ArbitragemViewModel());
        }

        [HttpPost("criar"), ValidateAntiForgeryToken]
        public IActionResult Create(ArbitragemViewModel vm)
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

                    var arbitros = _arbitroService.ListarArbitros();
                    ViewBag.Arbitros = arbitros.Select(a => new ArbitroViewModel
                    {
                        IdArbitro = a.IdArbitro,
                        Nome = a.Nome
                    }).ToList();

                    return View(vm);
                }

                var dto = new ArbitragemDto
                {
                    IdPartida = vm.IdPartida,
                    IdArbitro = vm.IdArbitro,
                    Funcao = vm.Funcao
                };
                _arbitragemService.CriarArbitragem(dto);
                TempData["Sucesso"] = "Arbitragem criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar arbitragem");
                ModelState.AddModelError("", "Erro ao criar arbitragem.");
                return View(vm);
            }
        }

        [HttpGet("deletar")]
        public IActionResult Delete(int idPartida, int idArbitro)
        {
            try
            {
                var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
                if (arbitragem == null) return NotFound();

                var partida = _partidaService.ObterPorId(arbitragem.IdPartida);
                var arbitro = _arbitroService.ObterPorId(arbitragem.IdArbitro);
                
                var vm = new ArbitragemViewModel
                {
                    IdPartida = arbitragem.IdPartida,
                    IdArbitro = arbitragem.IdArbitro,
                    Funcao = arbitragem.Funcao,
                    NomeArbitro = arbitro?.Nome,
                    LocalPartida = partida?.Local,
                    DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm")
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter arbitragem");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("deletar"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int idPartida, int idArbitro)
        {
            try
            {
                _arbitragemService.RemoverArbitragem(idPartida, idArbitro);
                TempData["Sucesso"] = "Arbitragem removida com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover arbitragem");
                TempData["Erro"] = "Erro ao remover arbitragem.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
