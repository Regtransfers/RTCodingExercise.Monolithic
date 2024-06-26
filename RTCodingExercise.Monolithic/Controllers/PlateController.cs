using RTCodingExercise.Monolithic.Business;
using RTCodingExercise.Monolithic.Common;
using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess;
using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.Controllers;

public class PlateController : Controller
{
    private readonly ILogger<PlateController> _logger;
    private readonly IPlatesProvider _platesProvider;
    
    public PlateController(ILogger<PlateController> logger, IPlatesProvider platesProvider)
    {
        _logger = logger;
        _platesProvider = platesProvider;
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Submit(Plate plate)
    {
        
        _logger.LogDebug("Submitting {plate}", plate);

        _platesProvider.AddPlate(plate);

        ViewBag.Saved = true;

        return View("Add", plate);
    }

    [HttpGet]
    public IActionResult ToggleReserve(Guid plateId, string searchString, string filter, int? page, SortOrderEnums? sortOrder)
    {
        _platesProvider.ToggleReserve(plateId);
        
        return RedirectToAction("Index", "Home", new
        {
            searchString,
            filter,
            page,
            sortOrder
        });
    }
}