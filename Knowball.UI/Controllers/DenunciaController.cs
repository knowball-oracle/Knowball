using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    [Route("[controller]")]
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
            _denunciaService = denunciaService ?? throw new ArgumentNullException(nameof(denunciaService));
            _partidaService = partidaService ?? throw new ArgumentNullException(nameof(partidaService));
            _arbitroService = arbitroService ?? throw new ArgumentNullException(nameof(arbitroService));
            _campeonatoService = campeonatoService ?? throw new ArgumentNullException(nameof(campeonatoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(""), HttpGet("Index"), Route("")]
        public IActionResult Index()
        {
            try
            {
                var denuncias = _denunciaService.ListarDenuncias();

                if (denuncias == null)
                {
                    _logger.LogWarning("Nenhuma denúncia encontrada");
                    return View(new List<DenunciaViewModel>());
                }

                var viewModels = new List<DenunciaViewModel>();

                foreach (var d in denuncias)
                {
                    try
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
                            DataPartida = partida?.DataPartida.ToString("dd/MM/yyyy HH:mm") ?? "N/A",
                            CampeonatoPartida = campeonato?.Nome ?? "N/A",
                            NomeArbitro = arbitro?.Nome ?? "N/A"
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Erro ao mapear denúncia {IdDenuncia}", d.IdDenuncia);
                        continue;
                    }
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar denúncias");
                // Retorna lista vazia ao invés de redirecionar
                return View(new List<DenunciaViewModel>());
            }
        }

        [HttpGet("criar")]
        public IActionResult Create()
        {
            CarregarDadosParaView();
            return View(new DenunciaViewModel());
        }

        [HttpPost("criar"), ValidateAntiForgeryToken]
        public IActionResult Create(DenunciaViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CarregarDadosParaView();
                    return View(vm);
                }

                // Gerar protocolo automaticamente se não fornecido
                if (string.IsNullOrEmpty(vm.Protocolo))
                {
                    vm.Protocolo = $"DEN{DateTime.Now:yyyyMMddHHmmss}";
                }

                var dto = new DenunciaDto
                {
                    Protocolo = vm.Protocolo,
                    Relato = vm.Relato,
                    Status = vm.Status ?? "Em Análise",
                    ResultadoAnalise = vm.ResultadoAnalise ?? string.Empty,
                    IdPartida = vm.IdPartida,
                    IdArbitro = vm.IdArbitro,
                    DataDenuncia = vm.DataDenuncia ?? DateTime.Now
                };

                _denunciaService.CriarDenuncia(dto);
                _logger.LogInformation($"Denúncia '{vm.Protocolo}' criada com sucesso");
                TempData["Sucesso"] = "Denúncia criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar denúncia");
                ModelState.AddModelError("", "Erro ao criar denúncia.");
                CarregarDadosParaView();
                return View(vm);
            }
        }

        [HttpGet("editar/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var denuncia = _denunciaService.ObterPorId(id);
                if (denuncia == null) return NotFound();

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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter denúncia {id}");
                TempData["Erro"] = "Erro ao carregar denúncia";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("editar/{id}"), ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DenunciaViewModel vm)
        {
            try
            {
                if (id != vm.IdDenuncia) return BadRequest();
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
                _logger.LogInformation($"Denúncia {id} atualizada com sucesso");
                TempData["Sucesso"] = "Denúncia atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar denúncia {id}");
                ModelState.AddModelError("", "Erro ao atualizar denúncia.");
                CarregarDadosParaView();
                return View(vm);
            }
        }

        [HttpGet("detalhes/{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                var denuncia = _denunciaService.ObterPorId(id);
                if (denuncia == null) return NotFound();

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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter detalhes da denúncia {id}");
                TempData["Erro"] = "Erro ao carregar denúncia";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet("deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var denuncia = _denunciaService.ObterPorId(id);
                if (denuncia == null) return NotFound();

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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter denúncia {id}");
                TempData["Erro"] = "Erro ao carregar denúncia";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("deletar/{id}"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _denunciaService.RemoverDenuncia(id);
                _logger.LogInformation($"Denúncia {id} deletada com sucesso");
                TempData["Sucesso"] = "Denúncia deletada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar denúncia {id}");
                TempData["Erro"] = "Erro ao deletar denúncia.";
                return RedirectToAction(nameof(Index));
            }
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
