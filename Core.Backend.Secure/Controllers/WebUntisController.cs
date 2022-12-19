using Core.Backend.Secure.Services;
using Core.WebUntis.Interface.Types;
using Microsoft.AspNetCore.Mvc;

namespace Core.Backend.Secure.Controllers;

[ApiController]
[Route("[controller]")]
public class WebUntisController : ControllerBase
{
    private WebUntisService _webUntisService;

    public WebUntisController(WebUntisService webUntisService)
    {
        _webUntisService = webUntisService;
    }

    [HttpGet("Teachers")]
    public async Task<IEnumerable<Teacher>> GetTeachers()
    {
        var teachers = await _webUntisService.GetTeachers();
        return teachers;
    }

    [HttpGet("Students")]
    public async Task<IEnumerable<Student>> GetStudents()
    {
        var students = await _webUntisService.GetStudents();
        return students;
    }

    [HttpGet("Homeworks")]
    public async Task<IEnumerable<Homework>> GetHomeworks(DateTime startDate, DateTime endDate)
    {
        var homeworks = await _webUntisService.GetHomeworks(startDate, endDate);
        return homeworks;
    }

    [HttpGet("Holidays")]
    public async Task<IEnumerable<Holiday>> GetHolidays()
    {
        var holidays = await _webUntisService.GetHolidays(User);
        return holidays;
    }

    [HttpGet("Subjects")]
    public async Task<IEnumerable<Subject>> GetSubjects()
    {
        return await _webUntisService.GetSubject(User);
    }

    [HttpGet("Rooms")]
    public async Task<IEnumerable<Room>> GetRooms()
    {
        return await _webUntisService.GetRooms();
    }

    [HttpGet("TeacherTimetable")]
    public async Task<IEnumerable<Timetable>> GetTimetableFromTeacher(DateTime startDate, DateTime endDate, int? personId)
    {
        return await _webUntisService.GetTimetableFromTeacher(startDate, endDate, personId);
    }

    [HttpGet("StudentTimetable")]
    public async Task<IEnumerable<Timetable>> GetTimetableFromStudent(DateTime startDate, DateTime endDate, int? personId)
    {
        return await _webUntisService.GetTimetableFromStudent(startDate, endDate, personId);
    }

    //[Authorize(Policy = "Auth-Token")]
}
