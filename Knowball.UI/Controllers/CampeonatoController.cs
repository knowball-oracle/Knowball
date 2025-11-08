using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    public class CampeonatoController : Controller
    {
        private readonly ICampeonatoService _service;
        private readonly ILogger<CampeonatoController> _logger;

        public CampeonatoController(ICampeonatoService service, ILogger<CampeonatoController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: Campeonato/Index
        public IActionResult Index()
        {
            try
            {
                var campeonatos = _service.ListarCampeonatos();
                var viewModels = campeonatos.Select(c => new CampeonatoViewModel
                {
                    IdCampeonato = c.IdCampeonato,
                    Nome = c.Nome,
                    Ano = c.Ano,
                    Categoria = c.Categoria
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar campeonatos");
                TempData["Erro"] = "Erro ao listar campeonatos";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Campeonato/Create
        public IActionResult Create() => View(new CampeonatoViewModel());

        // POST: Campeonato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CampeonatoViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return View(vm);

                var dto = new CampeonatoDto { Nome = vm.Nome, Ano = vm.Ano, Categoria = vm.Categoria };
                _service.CriarCampeonato(dto);
                _logger.LogInformation("Campeonato '{Nome}' criado com sucesso", vm.Nome);
                TempData["Sucesso"] = "Campeonato criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar campeonato");
                ModelState.AddModelError("", "Erro ao criar campeonato.");
                return View(vm);
            }
        }

        // GET: Campeonato/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var campeonato = _service.ObterPorId(id);
                if (campeonato == null)
                {
                    TempData["Erro"] = "Campeonato não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var vm = new CampeonatoViewModel
                {
                    IdCampeonato = campeonato.IdCampeonato,
                    Nome = campeonato.Nome,
                    Ano = campeonato.Ano,
                    Categoria = campeonato.Categoria
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter campeonato {Id}", id);
                TempData["Erro"] = "Erro ao carregar campeonato.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Campeonato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CampeonatoViewModel vm)
        {
            try
            {
                if (id != vm.IdCampeonato)
                {
                    TempData["Erro"] = "ID inválido.";
                    return RedirectToAction(nameof(Index));
                }

                if (!ModelState.IsValid) return View(vm);

                var dto = new CampeonatoDto
                {
                    IdCampeonato = vm.IdCampeonato,
                    Nome = vm.Nome,
                    Ano = vm.Ano,
                    Categoria = vm.Categoria
                };

                _service.AtualizarCampeonato(id, dto);
                _logger.LogInformation("Campeonato {Id} atualizado com sucesso", id);
                TempData["Sucesso"] = "Campeonato atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar campeonato {Id}", id);
                ModelState.AddModelError("", "Erro ao atualizar campeonato.");
                return View(vm);
            }
        }

        // GET: Campeonato/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var campeonato = _service.ObterPorId(id);
                if (campeonato == null)
                {
                    TempData["Erro"] = "Campeonato não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var vm = new CampeonatoViewModel
                {
                    IdCampeonato = campeonato.IdCampeonato,
                    Nome = campeonato.Nome,
                    Ano = campeonato.Ano,
                    Categoria = campeonato.Categoria
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter campeonato {Id}", id);
                TempData["Erro"] = "Erro ao carregar campeonato.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Campeonato/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.RemoverCampeonato(id);
                _logger.LogInformation("Campeonato {Id} deletado com sucesso", id);
                TempData["Sucesso"] = "Campeonato deletado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar campeonato {Id}", id);
                TempData["Erro"] = "Erro ao deletar campeonato.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
