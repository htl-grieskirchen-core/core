using System.Security.Claims;
using Core.AuthLib;
using Core.Database;
using Core.WebUntis.Implementation;
using Core.WebUntis.Interface.Types;
using Student = Core.WebUntis.Interface.Types.Student;

namespace Core.Backend.Secure.Services;

public class WebUntisService
{
    private readonly CredService _credService;
    private CoreContext _db;

    public WebUntisService(CredService credService, CoreContext db)
    {
        _credService = credService;
        _db = db;
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
            throw new NoLdapPasswordException();

        return new WebUntisCredentials
        {
            Username = user.GetUsername(),
            Password = password
        };
    }

    public IEnumerable<TeacherDTO> GetTeachers()
    {
        return _db.Teachers.Select(t => new TeacherDTO
        {
            Id = t.Id,
            Name = t.Name,
            FirstName = t.FirstName,
            LastName = t.LastName
        });

    }

    public async Task<IEnumerable<Student>> GetStudents(ClaimsPrincipal user)
    {
        var teachers = GetTeachers();
        var classes = await GetClasses(user);

        return _db.Students.ToList().Select(s => new Student
        {
            FirstName = s.FirstName,
            LastName = s.LastName,
            Email = s.Email,
            ClassId = classes.First(x => x.Name == s.Class).Id,
            ClassTeacherId = teachers.First(x => $"{x.FirstName} {x.LastName}" == s.ClassTeacher).Id
        });
    }

    public async Task<IEnumerable<Homework>> GetHomeworks(ClaimsPrincipal user, DateTime startDate, DateTime endDate)
    {
        var webUntisClient = await GetWebUntisClient(user);
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

    public async Task<IEnumerable<Room>> GetRooms(ClaimsPrincipal user)
    {
        var webUntisClient = await GetWebUntisClient(user);
        return await webUntisClient.GetRooms();
    }

    public async Task<IEnumerable<Class>> GetClasses(ClaimsPrincipal user)
    {
        var webUntisClient = await GetWebUntisClient(user);
        return await webUntisClient.GetClasses();
    }

    public async Task<IEnumerable<Timetable>> GetTimetableFromTeacher(ClaimsPrincipal user, DateTime startDate,
        DateTime endDate, int? personId)
    {
        var webUntisClient = await GetWebUntisClient(user);
        return await webUntisClient.GetTimetable(ElementType.Teacher, personId, startDate, endDate);
    }

    public async Task<IEnumerable<Timetable>> GetTimetableFromStudent(ClaimsPrincipal user, DateTime startDate,
        DateTime endDate, int? personId)
    {
        var webUntisClient = await GetWebUntisClient(user);
        return await webUntisClient.GetTimetable(ElementType.Student, personId, startDate, endDate);
    }

    public async Task<IEnumerable<Timetable>> GetSubstitutionsFromStudent(ClaimsPrincipal user, DateTime startDate, DateTime endDate,
        int? personId)
    {
        var webUntisClient = await GetWebUntisClient(user);
        var items = await webUntisClient.GetTimetable(ElementType.Student, personId, startDate, endDate);
        return items.Where(x => x.SubstitutionText == "Supplierung");
    }

    public async Task<IEnumerable<Timetable>> GetSubstitutionsFromTeacher(ClaimsPrincipal user, DateTime startDate, DateTime endDate,
        int? personId)
    {
        var webUntisClient = await GetWebUntisClient(user);
        var items = await webUntisClient.GetTimetable(ElementType.Teacher, personId, startDate, endDate);
        return items.Where(x => x.SubstitutionText == "Supplierung");
    }


    private class WebUntisCredentials
    {
        public string Username = null!;
        public string Password = null!;
    }

    private class NoLdapPasswordException : Exception
    {
    }
}
