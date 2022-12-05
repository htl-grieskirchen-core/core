using System.Security.Claims;
using Core.Backend.Secure.Auth;
using Core.WebUntis.Implementation;
using Core.WebUntis.Interface.Types;

namespace Core.Backend.Secure.Services;

public class WebUntisService
{
    private readonly CredService _credService;

    public WebUntisService(CredService credService)
    {
        _credService = credService;
    }

    private async Task<WebUntisClient> GetWebUntisClient(ClaimsPrincipal? user = null)
    {
        var webUntisClient = new WebUntisClient("https://arche.webuntis.com", "htbla-grieskirchen", "Synopsis");
        if (user != null)
        {
            var webUntisCredentials = GetWebUntisCredentials(user);
            await webUntisClient.AuthenticateWithSecret(webUntisCredentials.Username, webUntisCredentials.Secret);
        }

        return webUntisClient;
    }

    private WebUntisCredentials GetWebUntisCredentials(ClaimsPrincipal user)
    {
        var guid = new Guid(user.GetUUID());

        var secret = _credService.GetWebUntisSecret(guid);

        if (secret == null)
            throw new NoSecretException();

        return new WebUntisCredentials
        {
            Username = user.GetUsername(),
            Secret = secret
        };
    }

    public async Task<IEnumerable<Teacher>> GetTeachers()
    {
        var webUntisClient = await GetWebUntisClient();
        var teachers = await webUntisClient.GetTeachers();
        return teachers;
    }

    public async Task<IEnumerable<Homework>> GetHomeworks(DateTime from, DateTime to)
    {
        var webUntisClient = await GetWebUntisClient();
        var homeworks = await webUntisClient.GetHomeworks(from, to);
        return homeworks;
    }

    public async Task<IEnumerable<Student>> GetStudents()
    {
        var webUntisClient = await GetWebUntisClient();
        var students = await webUntisClient.GetStudents();
        return students;
    }

    private class WebUntisCredentials
    {
        public string Username = null!;
        public string Secret = null!;
    }

    private class NoSecretException : Exception
    {
    }
}
