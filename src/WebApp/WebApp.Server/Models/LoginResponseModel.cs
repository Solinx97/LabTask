namespace WebApp.Server.Models;

public class LoginResponseModel
{
    public UserModel User { get; set; }

    public string Token { get; set; }
}
