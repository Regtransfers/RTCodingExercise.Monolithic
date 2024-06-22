using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess.Interfaces;

namespace RTCodingExercise.Monolithic.Business;

public class PlatesProvider : IPlatesProvider
{
    public const int PAGE_SIZE = 20;
    private readonly IRepository<Plate> _plateRepository;

    public PlatesProvider(IRepository<Plate> plateRepository)
    {
        _plateRepository = plateRepository;
    }

    public async Task<PaginatedList<Plate>> GetAllAsync(int? pageIndex = 1)
    {
        var result =
            await PaginatedList<Plate>.CreateAsync(_plateRepository.Get(), pageIndex ?? 1, PAGE_SIZE);
        return result;
    }
}