using RTCodingExercise.Monolithic.Models;

namespace RTCodingExercise.Monolithic.Business;

public interface IPlatesProvider
{
    IEnumerable<Plate> GetAll();
}