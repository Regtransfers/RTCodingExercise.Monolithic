using Microsoft.Extensions.DependencyInjection;
using RTCodingExercise.Monolithic.DataAccess.Extensions;

namespace RTCodingExercise.Monolithic.Business.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddTransient<IPlatesProvider, PlatesProvider>();

        services.AddDataAccessServices();

        return services;
    }
}