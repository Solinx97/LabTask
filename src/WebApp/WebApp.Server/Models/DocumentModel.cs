using LabTask.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Server.Models;

public record DocumentModel(
    [Required] Guid Id, 
    [Required][StringLength(Document.NAME_MAX_LENGTH)] string Name, 
    [Required] string Description, 
    [Required] DateTimeOffset ExpireAt,
    [Required] Guid UserId
    );