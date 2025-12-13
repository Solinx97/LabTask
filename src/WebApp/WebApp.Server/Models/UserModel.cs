using System.ComponentModel.DataAnnotations;

namespace WebApp.Server.Models;

public record UserModel(
    [Required] Guid Id,
    [Required] string Email
    );
