using RTCodingExercise.Monolithic.Common.Models;

namespace RTCodingExercise.Monolithic.Business;

public interface IPlatesProvider
{
    Task<PaginatedList<Plate>> GetAllAsync(int? pageIndex = 1);
}