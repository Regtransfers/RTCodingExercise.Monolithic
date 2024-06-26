using RTCodingExercise.Monolithic.Common;
using RTCodingExercise.Monolithic.Common.Models;

namespace RTCodingExercise.Monolithic.Business;

public interface IPlatesProvider
{
    Task<PaginatedList<Plate>> GetAllAsync(string? searchFilter = null, int? pageIndex = 1, SortOrderEnums? sortOrder = null);
    void AddPlate(Plate plate);
    void ToggleReserve(Guid plateId);
}