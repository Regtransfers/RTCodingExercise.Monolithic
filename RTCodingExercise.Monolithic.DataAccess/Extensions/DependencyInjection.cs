using Microsoft.Extensions.DependencyInjection;
using RTCodingExercise.Monolithic.DataAccess.Interfaces;

namespace RTCodingExercise.Monolithic.DataAccess.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}