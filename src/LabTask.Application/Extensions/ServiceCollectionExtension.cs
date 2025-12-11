using LabTask.Application.Commands.CreateDocument;
using LabTask.Application.Queries.GetDocument;
using Microsoft.Extensions.DependencyInjection;

namespace LabTask.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApp(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateDocumentCommand).Assembly));

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetDocumentQuery).Assembly));
    }
}
