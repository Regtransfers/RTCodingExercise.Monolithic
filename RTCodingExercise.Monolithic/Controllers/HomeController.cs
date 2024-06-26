using Microsoft.AspNetCore.Mvc;
using RTCodingExercise.Monolithic.Models;
using System.Diagnostics;
using RTCodingExercise.Monolithic.Business;
using RTCodingExercise.Monolithic.Common;
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

        public async Task<IActionResult> Index(string searchString, string filter, int? page, SortOrderEnums? sortOrder)
        {
            //page = ResetSearchOrApplyExistingFilter(searchString, filter) ?? page;
            ViewBag.filter = searchString;
            
            var plates = await _platesProvider.GetAllAsync(searchString, page, sortOrder);

            return View(plates);
        }

        private int? ResetSearchOrApplyExistingFilter(string searchString, string filter)
        {
            int? page = null;
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                page = 1;
            }
            else
            {
                searchString = filter;
            }

            ViewBag.filter = searchString;
            return page;
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