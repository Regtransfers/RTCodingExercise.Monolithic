using RTCodingExercise.Monolithic.DataAccess.Interfaces;
using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.Business;

public class PlatesProvider : IPlatesProvider
{
    private readonly IRepository<Plate> _plateRepository;

    public PlatesProvider(IRepository<Plate> plateRepository)
    {
        _plateRepository = plateRepository;
    }

    public IEnumerable<Plate> GetAll()
    {
        return _plateRepository.Get();
    }
}