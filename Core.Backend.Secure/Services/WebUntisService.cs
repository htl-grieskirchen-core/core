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
            await webUntisClient.Authenticate(webUntisCredentials.Username, webUntisCredentials.Password);
        }

        return webUntisClient;
    }

    private WebUntisCredentials GetWebUntisCredentials(ClaimsPrincipal user)
    {
        var guid = user.GetUUID();

        var password = _credService.GetLdapPassword(guid);

        if (password == null)
            throw new NoSecretException();

        return new WebUntisCredentials
        {
            Username = user.GetUsername(),
            Password = password
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

    public async Task<IEnumerable<Holiday>> GetHolidays(ClaimsPrincipal user)
    {
        var webUntisClient = await GetWebUntisClient(user);
        var holidays = await webUntisClient.GetHolidays();
        return holidays;
    }

    public async Task<IEnumerable<Subject>> GetSubject(ClaimsPrincipal user)
    {
        var webUntisClient = await GetWebUntisClient(user);
        return await webUntisClient.GetSubjects();
    }

    public async Task<IEnumerable<Room>> GetRooms()
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetRooms();
    }

    public async Task<IEnumerable<Timetable>> GetTimetableFromTeacher(DateTime startDate, DateTime endDate, int? personId)
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetTimetable(ElementType.Teacher, personId, startDate, endDate);
    }

    public async Task<IEnumerable<Timetable>> GetTimetableFromStudent(DateTime startDate, DateTime endDate, int? personId)
    {
        var webUntisClient = await GetWebUntisClient();
        return await webUntisClient.GetTimetable(ElementType.Student, personId, startDate, endDate);
    }


    private class WebUntisCredentials
    {
        public string Username = null!;
        public string Password = null!;
    }

    private class NoSecretException : Exception
    {
    }
}
