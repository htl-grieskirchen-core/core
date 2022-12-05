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

    [HttpGet]
    public async Task<List<Teacher>> GetTeachers()
    {
        var teachers = await _webUntisService.GetTeachers();
        return teachers;
    }

    [HttpGet("Subjects")]
    public async Task<List<Subject>> GetSubjects()
    {
        return await _webUntisService.GetSubject();
    }

    [HttpGet("Rooms")]
    public async Task<List<Room>> GetRooms()
    {
        return await _webUntisService.GetRooms();
    }

    [HttpGet("TeacherTimetable")]
    public async Task<List<Timetable>> GetTimetableFromTeacher(DateTime startDate, DateTime endDate, int? personId)
    {
        return await _webUntisService.GetTimetableFromTeacher(startDate, endDate, personId);
    }
    [HttpGet("StudentTimetable")]
    public async Task<List<Timetable>> GetTimetableFromStudent(DateTime startDate, DateTime endDate, int? personId)
    {
        return await _webUntisService.GetTimetableFromStudent(startDate, endDate, personId);
    }


    //[Authorize(Policy = "Auth-Token")]
}
