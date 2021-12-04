using System.ComponentModel.DataAnnotations;
using DevPiraStudents.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiminalApis.Validators;

//ConfigureServices
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StudentContext>(option => option.UseSqlite("Data Source=DevPiraStudents"));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevPira Students", Version = "v1" });
            });
//Configure
using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevPira Students v1"));
}


app.MapStudentEndpoint();

await app.RunAsync();

public record Student
{
    public Guid Id { get; init; }
    [Required]
    public string FirstName { get; init; }
    [Required]
    public string LastName { get; init; }
    public bool Active { get; init; }
}

public class StudentContext : DbContext
{
    public StudentContext(DbContextOptions<StudentContext> options) : base(options)
    {

    }

    public DbSet<Student> Student => Set<Student>();
}

public interface IStudentRepository
{
    IEnumerable<Student> GetAll();
    Student Get(Guid id);

    void Add(Student student);
}

public class StudentRepository : IStudentRepository
{
    private readonly StudentContext _context;

    public StudentRepository(StudentContext context) => _context = context;
    public void Add(Student student)
    {
        _context.Student.Add(student);
        _context.SaveChanges();
    }

    public Student Get(Guid id) => _context.Student.Find(id);

    public IEnumerable<Student> GetAll() => _context.Student.ToList();
}