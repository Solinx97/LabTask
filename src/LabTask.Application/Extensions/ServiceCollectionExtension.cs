using LabTask.Application.Commands.CreateDocument;
using LabTask.Application.Commands.DeleteDocument;
using LabTask.Application.Commands.UpdateDocument;
using LabTask.Application.Queries.GetAllDocuments;
using LabTask.Application.Queries.GetDocument;
using LabTask.Application.Queries.GetDocumentsByUserId;
using Microsoft.Extensions.DependencyInjection;

namespace LabTask.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApp(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateDocumentCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(UpdateDocumentCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DeleteDocumentCommand).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetDocumentQuery).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetAllDocumentsQuery).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetDocumentsByUserIdQuery).Assembly));
    }
}
