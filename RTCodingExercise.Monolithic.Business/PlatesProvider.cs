using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using RTCodingExercise.Monolithic.Common;
using RTCodingExercise.Monolithic.Common.Models;
using RTCodingExercise.Monolithic.DataAccess.Interfaces;

namespace RTCodingExercise.Monolithic.Business;

public class PlatesProvider : IPlatesProvider
{
    public const int PAGE_SIZE = 20;

    private readonly ILogger<PlatesProvider> _logger;
    private readonly IRepository<Plate> _plateRepository;

    public PlatesProvider(IRepository<Plate> plateRepository, ILogger<PlatesProvider> logger)
    {
        _plateRepository = plateRepository;
        _logger = logger;
    }

    public async Task<PaginatedList<Plate>> GetAllAsync(string? searchFilter = null, int? pageIndex = 1, SortOrderEnums? sortOrder = null)
    {
        Expression<Func<Plate, bool>> filter = null;
            
        if (!string.IsNullOrWhiteSpace(searchFilter))
        {
            filter = plate => plate.Registration.Contains(searchFilter);
        }
        
        Func<IQueryable<Plate>, IOrderedQueryable<Plate>> orderBy = sortOrder 
        switch
        {
            SortOrderEnums.Ascending => t => t.OrderBy(p => p.MarkUp),
            SortOrderEnums.Descending => t => t.OrderByDescending(p => p.MarkUp),
            _ => null
        };

        return 
            await PaginatedList<Plate>.CreateAsync(_plateRepository.Get(
                    filter: filter,
                    orderBy: orderBy), 
                pageIndex ?? 1, PAGE_SIZE);
    }

    public void AddPlate(Plate plate)
    {
        var numbers = Regex.Match(plate.Registration, @"\d+").Value;
        if (!string.IsNullOrWhiteSpace(numbers))
            plate.Numbers = int.Parse(numbers);
        
        var letters = Regex.Replace(plate.Registration, @"[\d-]", string.Empty);
        plate.Letters = letters.ToUpper();
        
        _plateRepository.Insert(plate);
        _plateRepository.Save();
    }

    public void ToggleReserve(Guid plateId)
    {
        var plate = _plateRepository.GetById(plateId);

        if (plate == null)
        {
            throw new KeyNotFoundException($"No plate exists with Id of: {plateId}");
        }

        plate.Reserved = !plate.Reserved;

        _logger.LogInformation("Plate with Id {plateId} ({registration}) was set to reserved = {reservationStatus}", plate.Id,
            plate.Registration, plate.Reserved);
        
        _plateRepository.Update(plate);
        _plateRepository.Save();
    }
}