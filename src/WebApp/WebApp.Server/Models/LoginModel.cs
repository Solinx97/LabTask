using System.ComponentModel.DataAnnotations;

namespace WebApp.Server.Models;

public record LoginModel(
    [Required] string Email,
    [Required] string Password
    );
