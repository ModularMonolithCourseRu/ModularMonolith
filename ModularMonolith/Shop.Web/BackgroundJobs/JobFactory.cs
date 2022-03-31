using FluentScheduler;

namespace Shop.Web.BackgroundJobs;

public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob GetJobInstance<T>() where T : IJob
    {
        var scope = _serviceProvider.CreateScope();
        var result = scope.ServiceProvider.GetRequiredService<T>();
        return result;
    }
}