using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;

namespace Ordering.Application;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());

        serviceCollection.AddTransient(typeof(IStreamPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        serviceCollection.AddTransient(typeof(IStreamPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return serviceCollection;
    }
}