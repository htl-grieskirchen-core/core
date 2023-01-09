using Core.Database;

namespace Core.Backend.Secure.Services;

public class CSVReaderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CSVReaderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CoreContext>();
        db.Database.EnsureCreated();
        db.Teachers.RemoveRange(db.Teachers.ToList());
        PopulateTeachers(db);
        db.Students.RemoveRange(db.Students.ToList());
        PopulateStudents(db);
        return Task.Run(() => { }, stoppingToken);
    }

    private void PopulateStudents(CoreContext db)
    {
        try
        {
            File.ReadLines("CSV/students.csv")
                .Skip(1)
                .Select(x => x.Split(";"))
                .Select(student => new Student()
                {
                    Class = student[0],
                    ClassTeacher = student[1],
                    LastName = student[2],
                    FirstName = student[3],
                    Email = student[4]
                }
                )
                .ToList()
                .ForEach(x => db.Students.Add(x));
            db.SaveChanges();
        }
        catch (FileNotFoundException e)
        {
            throw new Exception("File \"students.csv\" does not exist!");
        }


    }

    private static void PopulateTeachers(CoreContext db)
    {
        try
        {
            File.ReadLines("CSV/teachers.csv")
                .Skip(1)
                .Select(x => x.Split(";"))
                .Select(x => new Teacher
                {
                    Id = Int32.Parse(x[0]),
                    Name = x[1],
                    LastName = x[2],
                    FirstName = x[3]
                }
                ).ToList().ForEach(t => db.Teachers.Add(t));
            db.SaveChanges();
        }
        catch (FileNotFoundException e)
        {
            throw new Exception("File \"teachers.csv\" does not exist!");
        }
    }
}
