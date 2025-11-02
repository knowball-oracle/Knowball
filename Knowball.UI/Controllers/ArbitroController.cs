using Knowball.Application.DTOs;
using Knowball.Application.Services;
using Knowball.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    [Route("[controller]")]
    public class ArbitroController : Controller
    {
        private readonly IArbitroService _service;
        private readonly ILogger<ArbitroController> _logger;

        public ArbitroController(IArbitroService service, ILogger<ArbitroController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(""), HttpGet("Index"), Route("")]
        public IActionResult Index()
        {
            try
            {
                var arbitros = _service.ListarArbitros();
                var viewModels = arbitros.Select(a => new ArbitroViewModel
                {
                    IdArbitro = a.IdArbitro,
                    Nome = a.Nome,
                    DataNascimento = a.DataNascimento,
                    Status = a.Status
                }).ToList();

                return View(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar árbitros");
                TempData["Erro"] = "Erro ao listar árbitros";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet("criar")]
        public IActionResult Create() => View(new ArbitroViewModel());

        [HttpPost("criar"), ValidateAntiForgeryToken]
        public IActionResult Create(ArbitroViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid) return View(vm);

                var dto = new ArbitroDto
                {
                    Nome = vm.Nome,
                    DataNascimento = vm.DataNascimento,
                    Status = vm.Status
                };
                _service.CriarArbitro(dto);
                TempData["Sucesso"] = "Árbitro criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar árbitro");
                ModelState.AddModelError("", "Erro ao criar árbitro.");
                return View(vm);
            }
        }

        [HttpGet("editar/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var arbitro = _service.ObterPorId(id);
                if (arbitro == null) return NotFound();

                var vm = new ArbitroViewModel
                {
                    IdArbitro = arbitro.IdArbitro,
                    Nome = arbitro.Nome,
                    DataNascimento = arbitro.DataNascimento,
                    Status = arbitro.Status
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter árbitro {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("editar/{id}"), ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ArbitroViewModel vm)
        {
            try
            {
                if (id != vm.IdArbitro) return BadRequest();
                if (!ModelState.IsValid) return View(vm);

                var dto = new ArbitroDto
                {
                    IdArbitro = vm.IdArbitro,
                    Nome = vm.Nome,
                    DataNascimento = vm.DataNascimento,
                    Status = vm.Status
                };
                _service.AtualizarArbitro(id, dto);
                TempData["Sucesso"] = "Árbitro atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar árbitro {id}");
                ModelState.AddModelError("", "Erro ao atualizar árbitro.");
                return View(vm);
            }
        }

        [HttpGet("deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var arbitro = _service.ObterPorId(id);
                if (arbitro == null) return NotFound();

                var vm = new ArbitroViewModel
                {
                    IdArbitro = arbitro.IdArbitro,
                    Nome = arbitro.Nome,
                    DataNascimento = arbitro.DataNascimento,
                    Status = arbitro.Status
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter árbitro {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost("deletar/{id}"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.RemoverArbitro(id);
                TempData["Sucesso"] = "Árbitro deletado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar árbitro {id}");
                TempData["Erro"] = "Erro ao deletar árbitro.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
