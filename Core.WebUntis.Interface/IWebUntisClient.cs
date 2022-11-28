using Core.WebUntis.Interface.Types;

namespace Core.WebUntis.Interface;

public interface IWebUntisClient
{
    public Task<Authentication> Authenticate(string user, string password);
    public Task<Authentication> AuthenticateWithSecret(string user, string secret);
    public Task<List<Class>> GetClasses(int schoolYear);
    public Task<List<Subject>> GetSubjects();
    public Task<List<Room>> GetRooms();
    public Task<IEnumerable<Homework>> GetHomeworks(DateTime startDate, DateTime endDate);
    public Task<IEnumerable<Holiday>> GetHolidays();
    [Obsolete("Deprecated due to insufficient rights")]
    public Task<List<Student>> GetStudents();
    public List<Teacher> GetTeachers();
    public Task<List<Timetable>> GetTimetable(ElementType type, int? personId, DateTime startDate, DateTime endDate);
    [Obsolete("Deprecated due to unknown documentation")]
    public Task<List<ExamType>> GetExamTypes();
    [Obsolete("Deprecated due to insufficient rights")]
    public Task<List<Exam>> GetExams(int examTypeId, DateTime startDate, DateTime endDate);
}
