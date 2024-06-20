using Microsoft.AspNetCore.Mvc;
using RTCodingExercise.Monolithic.Models;
using System.Diagnostics;
using RTCodingExercise.Monolithic.Business;
using RTCodingExercise.Monolithic.DataAccess;

namespace RTCodingExercise.Monolithic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlatesProvider _platesProvider;

        public HomeController(ILogger<HomeController> logger, IPlatesProvider platesProvider)
        {
            _logger = logger;
            _platesProvider = platesProvider;
        }

        public IActionResult Index()
        {
            var plates = _platesProvider.GetAll().ToList();

            return View(plates);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}