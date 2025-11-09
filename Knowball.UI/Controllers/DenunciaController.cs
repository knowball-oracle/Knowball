using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    public class DenunciaController : Controller
    {
        private readonly IDenunciaService _denunciaService;
        private readonly IPartidaService _partidaService;
        private readonly IArbitroService _arbitroService;
        private readonly ICampeonatoService _campeonatoService;
        private readonly ILogger<DenunciaController> _logger;

        public DenunciaController(IDenunciaService denunciaService, IPartidaService partidaService,
            IArbitroService arbitroService, ICampeonatoService campeonatoService, ILogger<DenunciaController> logger)
        {
            _denunciaService = denunciaService;
            _partidaService = partidaService;
            _arbitroService = arbitroService;
            _campeonatoService = campeonatoService;
            _logger = logger;
        }

        // GET: Denuncia
        public IActionResult Index()
        {
            var denuncias = _denunciaService.ListarDenuncias() ?? Enumerable.Empty<DenunciaDto>();
            var viewModels = new List<DenunciaViewModel>();
            foreach (var d in denuncias)
            {
                var partida = _partidaService.ObterPorId(d.IdPartida);
                var arbitro = _arbitroService.ObterPorId(d.IdArbitro);
                var campeonato = partida != null ? _campeonatoService.ObterPorId(partida.IdCampeonato) : null;
                viewModels.Add(new DenunciaViewModel
                {
                    IdDenuncia = d.IdDenuncia,
                    Protocolo = d.Protocolo,
                    Relato = d.Relato,
                    Status = d.Status,
                    ResultadoAnalise = d.ResultadoAnalise,
                    IdPartida = d.IdPartida,
                    IdArbitro = d.IdArbitro,
                    DataDenuncia = d.DataDenuncia,
                    LocalPartida = partida?.Local ?? "N/A",
                    DataPartida = partida != null ? partida.DataPartida.ToString("dd/MM/yyyy HH:mm") : "N/A",
                    CampeonatoPartida = campeonato?.Nome ?? "N/A",
                    NomeArbitro = arbitro?.Nome ?? "N/A"
                });
            }
            return View(viewModels);
        }

        // GET: Denuncia/Create
        public IActionResult Create()
        {
            CarregarDadosParaView();
            return View(new DenunciaViewModel());
        }

        // POST: Denuncia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DenunciaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                CarregarDadosParaView();
                return View(vm);
            }
            var dto = new DenunciaDto
            {
                Protocolo = string.IsNullOrEmpty(vm.Protocolo) ? $"DEN{DateTime.Now:yyyyMMddHHmmss}" : vm.Protocolo,
                Relato = vm.Relato,
                Status = vm.Status ?? "Em Análise",
                ResultadoAnalise = vm.ResultadoAnalise ?? string.Empty,
                IdPartida = vm.IdPartida,
                IdArbitro = vm.IdArbitro,
                DataDenuncia = vm.DataDenuncia ?? DateTime.Now
            };
            _denunciaService.CriarDenuncia(dto);
            TempData["Sucesso"] = "Denúncia criada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Denuncia/Details/5
        public IActionResult Details(int id)
        {
            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
            {
                TempData["Erro"] = "Denúncia não encontrada.";
                return RedirectToAction(nameof(Index));
            }
            var partida = _partidaService.ObterPorId(denuncia.IdPartida);
            var arbitro = _arbitroService.ObterPorId(denuncia.IdArbitro);
            var campeonato = partida != null ? _campeonatoService.ObterPorId(partida.IdCampeonato) : null;

            var vm = new DenunciaViewModel
            {
                IdDenuncia = denuncia.IdDenuncia,
                Protocolo = denuncia.Protocolo,
                Relato = denuncia.Relato,
                Status = denuncia.Status,
                ResultadoAnalise = denuncia.ResultadoAnalise,
                IdPartida = denuncia.IdPartida,
                IdArbitro = denuncia.IdArbitro,
                DataDenuncia = denuncia.DataDenuncia,
                LocalPartida = partida?.Local,
                DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm"),
                CampeonatoPartida = campeonato?.Nome,
                NomeArbitro = arbitro?.Nome
            };

            return View(vm);
        }

        // GET: Denuncia/Edit/5
        public IActionResult Edit(int id)
        {
            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
            {
                TempData["Erro"] = "Denúncia não encontrada.";
                return RedirectToAction(nameof(Index));
            }
            var partida = _partidaService.ObterPorId(denuncia.IdPartida);
            var arbitro = _arbitroService.ObterPorId(denuncia.IdArbitro);
            var campeonato = partida != null ? _campeonatoService.ObterPorId(partida.IdCampeonato) : null;

            var vm = new DenunciaViewModel
            {
                IdDenuncia = denuncia.IdDenuncia,
                Protocolo = denuncia.Protocolo,
                Relato = denuncia.Relato,
                Status = denuncia.Status,
                ResultadoAnalise = denuncia.ResultadoAnalise,
                IdPartida = denuncia.IdPartida,
                IdArbitro = denuncia.IdArbitro,
                DataDenuncia = denuncia.DataDenuncia,
                LocalPartida = partida?.Local,
                DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm"),
                CampeonatoPartida = campeonato?.Nome,
                NomeArbitro = arbitro?.Nome
            };

            CarregarDadosParaView();
            return View(vm);
        }

        // POST: Denuncia/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DenunciaViewModel vm)
        {
            if (id != vm.IdDenuncia)
            {
                TempData["Erro"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                CarregarDadosParaView();
                return View(vm);
            }

            var dto = new DenunciaDto
            {
                IdDenuncia = vm.IdDenuncia,
                Protocolo = vm.Protocolo,
                Relato = vm.Relato,
                Status = vm.Status ?? "Em Análise",
                ResultadoAnalise = vm.ResultadoAnalise ?? string.Empty,
                IdPartida = vm.IdPartida,
                IdArbitro = vm.IdArbitro,
                DataDenuncia = vm.DataDenuncia ?? DateTime.Now
            };

            _denunciaService.AtualizarDenuncia(id, dto);
            TempData["Sucesso"] = "Denúncia atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Denuncia/Delete/5
        public IActionResult Delete(int id)
        {
            var denuncia = _denunciaService.ObterPorId(id);
            if (denuncia == null)
            {
                TempData["Erro"] = "Denúncia não encontrada.";
                return RedirectToAction(nameof(Index));
            }
            var partida = _partidaService.ObterPorId(denuncia.IdPartida);
            var arbitro = _arbitroService.ObterPorId(denuncia.IdArbitro);
            var campeonato = partida != null ? _campeonatoService.ObterPorId(partida.IdCampeonato) : null;

            var vm = new DenunciaViewModel
            {
                IdDenuncia = denuncia.IdDenuncia,
                Protocolo = denuncia.Protocolo,
                Relato = denuncia.Relato,
                Status = denuncia.Status,
                ResultadoAnalise = denuncia.ResultadoAnalise,
                IdPartida = denuncia.IdPartida,
                IdArbitro = denuncia.IdArbitro,
                DataDenuncia = denuncia.DataDenuncia,
                LocalPartida = partida?.Local,
                DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm"),
                CampeonatoPartida = campeonato?.Nome,
                NomeArbitro = arbitro?.Nome
            };

            return View(vm);
        }

        // POST: Denuncia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _denunciaService.RemoverDenuncia(id);
            TempData["Sucesso"] = "Denúncia deletada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private void CarregarDadosParaView()
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
                Nome = a.Nome,
                Status = a.Status
            }).ToList();
        }
    }
}
