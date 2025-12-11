using System.ComponentModel.DataAnnotations;

namespace WebApp.Server.Models;

public record RegistrationModel(
    [Required] string Email,
    [Required] string Password
    );