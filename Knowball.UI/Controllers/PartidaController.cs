using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    [Route("[controller]")]
    public class PartidaController : Controller
    {
        private readonly IPartidaService _partidaService;
        private readonly ICampeonatoService _campeonatoService;
        private readonly IEquipeService _equipeService;
        private readonly ILogger<PartidaController> _logger;

        public PartidaController(IPartidaService partidaService, ICampeonatoService campeonatoService,
            IEquipeService equipeService, ILogger<PartidaController> logger)
        {
            _partidaService = partidaService ?? throw new ArgumentNullException(nameof(partidaService));
            _campeonatoService = campeonatoService ?? throw new ArgumentNullException(nameof(campeonatoService));
            _equipeService = equipeService ?? throw new ArgumentNullException(nameof(equipeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(""), HttpGet("Index"), Route("")]
        public IActionResult Index()
        {
            try
            {
                var partidas = _partidaService.ListarPartidas();

                if (partidas == null)
                {
                    _logger.LogWarning("Nenhuma partida encontrada");
                    return View(new List<PartidaViewModel>());
                }

                var viewModels = new List<PartidaViewModel>();

                foreach (var p in partidas)
                {
                    try
                    {
                        var campeonato = _campeonatoService.ObterPorId(p.IdCampeonato);
                        var equipe1 = _equipeService.ObterPorId(p.IdEquipe1);
                        var equipe2 = _equipeService.ObterPorId(p.IdEquipe2);

                        viewModels.Add(new PartidaViewModel
                        {
                            IdPartida = p.IdPartida,
                            IdCampeonato = p.IdCampeonato,
                            IdEquipe1 = p.IdEquipe1,
                            IdEquipe2 = p.IdEquipe2,
                            DataPartida = p.DataPartida,
                            Local = p.Local,
                            PlacarMandante = p.PlacarMandante,
                            PlacarVisitante = p.PlacarVisitante,
                            NomeCampeonato = campeonato?.Nome ?? "N/A",
                            NomeEquipe1 = equipe1?.Nome ?? "N/A",
                            NomeEquipe2 = equipe2?.Nome ?? "N/A"
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Erro ao mapear partida {IdPartida}", p.IdPartida);
                        continue;
                    }
                }

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar partidas");
                // Retorna lista vazia ao invés de redirecionar
                return View(new List<PartidaViewModel>());
            }
        }


        [HttpGet("criar")]
        public IActionResult Create()
        {
            CarregarDadosParaView();
            return View(new PartidaViewModel());
        }

        [HttpPost("criar"), ValidateAntiForgeryToken]
        public IActionResult Create(PartidaViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CarregarDadosParaView();
                    return View(vm);
                }

                // Validação: Equipes diferentes
                if (vm.IdEquipe1 == vm.IdEquipe2)
                {
                    ModelState.AddModelError("", "A equipe mandante deve ser diferente da visitante.");
                    CarregarDadosParaView();
                    return View(vm);
                }

                var dto = new PartidaDto
                {
                    IdCampeonato = vm.IdCampeonato,
                    IdEquipe1 = vm.IdEquipe1,
                    IdEquipe2 = vm.IdEquipe2,
                    DataPartida = vm.DataPartida,
                    Local = vm.Local,
                    PlacarMandante = vm.PlacarMandante,
                    PlacarVisitante = vm.PlacarVisitante
                };

                _partidaService.CriarPartida(dto);
                _logger.LogInformation("Partida criada com sucesso em {Local}", vm.Local);
                TempData["Sucesso"] = "Partida criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar partida");
                ModelState.AddModelError("", "Erro ao criar partida.");
                CarregarDadosParaView();
                return View(vm);
            }
        }

        [HttpGet("editar/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var partida = _partidaService.ObterPorId(id);
                if (partida == null) return NotFound();

                var campeonato = _campeonatoService.ObterPorId(partida.IdCampeonato);
                var equipe1 = _equipeService.ObterPorId(partida.IdEquipe1);
                var equipe2 = _equipeService.ObterPorId(partida.IdEquipe2);

                var vm = new PartidaViewModel
                {
                    IdPartida = partida.IdPartida,
                    IdCampeonato = partida.IdCampeonato,
                    IdEquipe1 = partida.IdEquipe1,
                    IdEquipe2 = partida.IdEquipe2,
                    DataPartida = partida.DataPartida,
                    Local = partida.Local,
                    PlacarMandante = partida.PlacarMandante,
                    PlacarVisitante = partida.PlacarVisitante,
                    NomeCampeonato = campeonato?.Nome,
                    NomeEquipe1 = equipe1?.Nome,
                    NomeEquipe2 = equipe2?.Nome
                };

                CarregarDadosParaView();
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter partida {Id}", id);
                TempData["Erro"] = "Erro ao carregar partida";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("editar/{id}"), ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PartidaViewModel vm)
        {
            try
            {
                if (id != vm.IdPartida) return BadRequest();
                if (!ModelState.IsValid)
                {
                    CarregarDadosParaView();
                    return View(vm);
                }

                // Validação: Equipes diferentes
                if (vm.IdEquipe1 == vm.IdEquipe2)
                {
                    ModelState.AddModelError("", "A equipe mandante deve ser diferente da visitante.");
                    CarregarDadosParaView();
                    return View(vm);
                }

                var dto = new PartidaDto
                {
                    IdPartida = vm.IdPartida,
                    IdCampeonato = vm.IdCampeonato,
                    IdEquipe1 = vm.IdEquipe1,
                    IdEquipe2 = vm.IdEquipe2,
                    DataPartida = vm.DataPartida,
                    Local = vm.Local,
                    PlacarMandante = vm.PlacarMandante,
                    PlacarVisitante = vm.PlacarVisitante
                };

                _partidaService.AtualizarPartida(id, dto);
                _logger.LogInformation("Partida {Id} atualizada com sucesso", id);
                TempData["Sucesso"] = "Partida atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar partida {Id}", id);
                ModelState.AddModelError("", "Erro ao atualizar partida.");
                CarregarDadosParaView();
                return View(vm);
            }
        }

        [HttpGet("deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var partida = _partidaService.ObterPorId(id);
                if (partida == null) return NotFound();

                var campeonato = _campeonatoService.ObterPorId(partida.IdCampeonato);
                var equipe1 = _equipeService.ObterPorId(partida.IdEquipe1);
                var equipe2 = _equipeService.ObterPorId(partida.IdEquipe2);

                var vm = new PartidaViewModel
                {
                    IdPartida = partida.IdPartida,
                    IdCampeonato = partida.IdCampeonato,
                    IdEquipe1 = partida.IdEquipe1,
                    IdEquipe2 = partida.IdEquipe2,
                    DataPartida = partida.DataPartida,
                    Local = partida.Local,
                    PlacarMandante = partida.PlacarMandante,
                    PlacarVisitante = partida.PlacarVisitante,
                    NomeCampeonato = campeonato?.Nome,
                    NomeEquipe1 = equipe1?.Nome,
                    NomeEquipe2 = equipe2?.Nome
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter partida {Id}", id);
                TempData["Erro"] = "Erro ao carregar partida";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("deletar/{id}"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _partidaService.RemoverPartida(id);
                _logger.LogInformation("Partida {Id} deletada com sucesso", id);
                TempData["Sucesso"] = "Partida deletada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar partida {Id}", id);
                TempData["Erro"] = "Erro ao deletar partida.";
                return RedirectToAction(nameof(Index));
            }
        }

        private void CarregarDadosParaView()
        {
            var campeonatos = _campeonatoService.ListarCampeonatos();
            ViewBag.Campeonatos = campeonatos.Select(c => new CampeonatoViewModel
            {
                IdCampeonato = c.IdCampeonato,
                Nome = c.Nome,
                Ano = c.Ano
            }).ToList();

            var equipes = _equipeService.ListarEquipes();
            ViewBag.Equipes = equipes.Select(e => new EquipeViewModel
            {
                IdEquipe = e.IdEquipe,
                Nome = e.Nome,
                Cidade = e.Cidade,
                Estado = e.Estado
            }).ToList();
        }
    }
}
