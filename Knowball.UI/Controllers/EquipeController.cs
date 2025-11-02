using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    [Route("[controller]")]
    public class EquipeController : Controller
    {
        private readonly IEquipeService _service;
        private readonly ILogger<EquipeController> _logger;

        public EquipeController(IEquipeService service, ILogger<EquipeController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(""), HttpGet("Index"), Route("")]
        public IActionResult Index()
        {
            try
            {
                var equipes = _service.ListarEquipes();
                var viewModels = equipes.Select(e => new EquipeViewModel
                {
                    IdEquipe = e.IdEquipe,
                    Nome = e.Nome,
                    Cidade = e.Cidade,
                    Estado = e.Estado
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar equipes");
                TempData["Erro"] = "Erro ao listar equipes";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("criar")]
        public IActionResult Create() => View(new EquipeViewModel());

        [HttpPost("criar"), ValidateAntiForgeryToken]
        public IActionResult Create(EquipeViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return View(vm);

                var dto = new EquipeDto { Nome = vm.Nome, Cidade = vm.Cidade, Estado = vm.Estado };
                _service.CriarEquipe(dto);
                TempData["Sucesso"] = "Equipe criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar equipe");
                ModelState.AddModelError("", "Erro ao criar equipe.");
                return View(vm);
            }
        }

        [HttpGet("editar/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var equipe = _service.ObterPorId(id);
                if (equipe == null) return NotFound();

                var vm = new EquipeViewModel
                {
                    IdEquipe = equipe.IdEquipe,
                    Nome = equipe.Nome,
                    Cidade = equipe.Cidade,
                    Estado = equipe.Estado
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter equipe {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("editar/{id}"), ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EquipeViewModel vm)
        {
            try
            {
                if (id != vm.IdEquipe) return BadRequest();
                if (!ModelState.IsValid) return View(vm);

                var dto = new EquipeDto { IdEquipe = vm.IdEquipe, Nome = vm.Nome, Cidade = vm.Cidade, Estado = vm.Estado };
                _service.AtualizarEquipe(id, dto);
                TempData["Sucesso"] = "Equipe atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar equipe {id}");
                ModelState.AddModelError("", "Erro ao atualizar equipe.");
                return View(vm);
            }
        }

        [HttpGet("deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var equipe = _service.ObterPorId(id);
                if (equipe == null) return NotFound();

                var vm = new EquipeViewModel
                {
                    IdEquipe = equipe.IdEquipe,
                    Nome = equipe.Nome,
                    Cidade = equipe.Cidade,
                    Estado = equipe.Estado
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter equipe {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("deletar/{id}"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.RemoverEquipe(id);
                TempData["Sucesso"] = "Equipe deletada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar equipe {id}");
                TempData["Erro"] = "Erro ao deletar equipe.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
