using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess.Interfaces;

namespace RTCodingExercise.Monolithic.Business;

public class PlatesProvider : IPlatesProvider
{
    private readonly IRepository<Plate> _plateRepository;

    public PlatesProvider(IRepository<Plate> plateRepository)
    {
        _plateRepository = plateRepository;
    }

    public async Task<PaginatedList<Plate>> GetAllAsync(int? pageIndex = 1)
    {
        var pageSize = 20;
        var result =
            await PaginatedList<Plate>.CreateAsync(_plateRepository.Get(), pageIndex ?? 1, pageSize);
        return result;
    }
}