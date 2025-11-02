using Knowball.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knowball.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICampeonatoService _campeonatoService;
        private readonly IEquipeService _equipeService;
        private readonly IArbitroService _arbitroService;
        private readonly IPartidaService _partidaService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICampeonatoService campeonatoService, IEquipeService equipeService,
            IArbitroService arbitroService, IPartidaService partidaService, ILogger<HomeController> logger)
        {
            _campeonatoService = campeonatoService;
            _equipeService = equipeService;
            _arbitroService = arbitroService;
            _partidaService = partidaService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var campeonatos = _campeonatoService.ListarCampeonatos().Count();
                var equipes = _equipeService.ListarEquipes().Count();
                var arbitros = _arbitroService.ListarArbitros().Count();
                var partidas = _partidaService.ListarPartidas().Count();

                ViewBag.TotalCampeonatos = campeonatos;
                ViewBag.TotalEquipes = equipes;
                ViewBag.TotalArbitros = arbitros;
                ViewBag.TotalPartidas = partidas;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar dashboard");
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
