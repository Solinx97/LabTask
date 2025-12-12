using LabTask.Application.Commands.CreateDocument;
using Microsoft.Extensions.DependencyInjection;

namespace LabTask.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddMediatorSource(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateDocumentCommand).Assembly));
    }
}