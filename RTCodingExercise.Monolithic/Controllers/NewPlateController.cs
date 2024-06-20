using RTCodingExercise.Monolithic.DataAccess;
using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.Controllers;

public class NewPlateController : Controller
{
    private readonly ILogger<NewPlateController> _logger;
    private readonly ApplicationDbContext _context;
    
    public NewPlateController(ILogger<NewPlateController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
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

        return View("Index", plate);
    }
}