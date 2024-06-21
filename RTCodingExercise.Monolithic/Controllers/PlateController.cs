using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess;
using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.Controllers;

public class PlateController : Controller
{
    private readonly ILogger<PlateController> _logger;
    private readonly ApplicationDbContext _context;
    
    public PlateController(ILogger<PlateController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Add()
    {
        _logger.LogDebug("Test");
        return View();
    }

    [HttpPost]
    public IActionResult Submit(Plate plate)
    {
        _logger.LogDebug("Submitting {plate}", plate);
        _context.Plates.Add(plate);
        _context.SaveChanges();
        ViewBag.Saved = true;

        return View("Add", plate);
    }
}