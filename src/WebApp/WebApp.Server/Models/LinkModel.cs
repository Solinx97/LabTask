using System.ComponentModel.DataAnnotations;

namespace WebApp.Server.Models;

public record LinkModel(
    [Required] Guid Id,
    [Required] Guid Uri,
    [Required] Guid DocumentId,
    [Required] Guid OwnerId,
    [Required] Guid ToUserId,
    [Required] DateTimeOffset ExpireAt
    );
