using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    public class ArbitragemController : Controller
    {
        private readonly IArbitragemService _arbitragemService;
        private readonly IPartidaService _partidaService;
        private readonly IArbitroService _arbitroService;
        private readonly ILogger<ArbitragemController> _logger;

        public ArbitragemController(
            IArbitragemService arbitragemService,
            IPartidaService partidaService,
            IArbitroService arbitroService,
            ILogger<ArbitragemController> logger)
        {
            _arbitragemService = arbitragemService ?? throw new ArgumentNullException(nameof(arbitragemService));
            _partidaService = partidaService ?? throw new ArgumentNullException(nameof(partidaService));
            _arbitroService = arbitroService ?? throw new ArgumentNullException(nameof(arbitroService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: Arbitragem/Index
        public IActionResult Index()
        {
            try
            {
                var arbitragens = _arbitragemService.ListarArbitragens();
                var viewModels = new List<ArbitragemViewModel>();

                foreach (var a in arbitragens)
                {
                    try
                    {
                        var partida = _partidaService.ObterPorId(a.IdPartida);
                        var arbitro = _arbitroService.ObterPorId(a.IdArbitro);

                        viewModels.Add(new ArbitragemViewModel
                        {
                            IdPartida = a.IdPartida,
                            IdArbitro = a.IdArbitro,
                            Funcao = a.Funcao,
                            NomeArbitro = arbitro?.Nome ?? "Não informado",
                            LocalPartida = partida?.Local ?? "Não informado",
                            DataPartida = partida != null ? partida.DataPartida.ToString("dd/MM/yyyy HH:mm") : "Não informado"
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Erro ao mapear arbitragem IdPartida: {IdPartida}, IdArbitro: {IdArbitro}",
                            a.IdPartida, a.IdArbitro);
                        continue;
                    }
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar arbitragens");
                TempData["Erro"] = "Erro ao listar arbitragens: " + ex.Message;
                return View(new List<ArbitragemViewModel>());
            }
        }


        // GET: Arbitragem/Create
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

        // POST: Arbitragem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                ModelState.AddModelError("", "Erro ao criar arbitragem: " + ex.Message);

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
        }

        // GET: Arbitragem/Delete
        public IActionResult Delete(int idPartida, int idArbitro)
        {
            try
            {
                var arbitragem = _arbitragemService.ObterPorIds(idPartida, idArbitro);
                if (arbitragem == null)
                {
                    TempData["Erro"] = "Arbitragem não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                var partida = _partidaService.ObterPorId(arbitragem.IdPartida);
                var arbitro = _arbitroService.ObterPorId(arbitragem.IdArbitro);

                var vm = new ArbitragemViewModel
                {
                    IdPartida = arbitragem.IdPartida,
                    IdArbitro = arbitragem.IdArbitro,
                    Funcao = arbitragem.Funcao,
                    NomeArbitro = arbitro?.Nome ?? "Não informado",
                    LocalPartida = partida?.Local ?? "Não informado",
                    DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm") ?? "Não informado"
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter arbitragem");
                TempData["Erro"] = "Erro ao carregar arbitragem.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Arbitragem/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
                _logger.LogError(ex, "Erro ao remover arbitragem");
                TempData["Erro"] = "Erro ao remover arbitragem: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
