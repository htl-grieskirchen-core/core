using System.Security.Claims;
using Core.AuthLib;
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
        var guid = user.GetUUID();

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

    public async Task<IEnumerable<Student>> GetStudents()
    {
        var webUntisClient = await GetWebUntisClient();
        var students = await webUntisClient.GetStudents();
        return students;
    }

    public async Task<IEnumerable<Homework>> GetHomeworks(DateTime startDate, DateTime endDate)
    {
        var webUntisClient = await GetWebUntisClient();
        var homeworks = await webUntisClient.GetHomeworks(startDate, endDate);
        return homeworks;
    }

    public async Task<IEnumerable<Holiday>> GetHolidays()
    {
        var webUntisClient = await GetWebUntisClient();
        var holidays = await webUntisClient.GetHolidays();
        return holidays;
    }

    public async Task<List<Subject>> GetSubject()
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetSubjects();
    }

    public async Task<List<Room>> GetRooms()
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetRooms();
    }

    public async Task<List<Timetable>> GetTimetableFromTeacher(DateTime startDate, DateTime endDate, int? personId)
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetTimetable(ElementType.Teacher, personId, startDate, endDate);
    }

    public async Task<List<Timetable>> GetTimetableFromStudent(DateTime startDate, DateTime endDate, int? personId)
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetTimetable(ElementType.Student, personId, startDate, endDate);
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
