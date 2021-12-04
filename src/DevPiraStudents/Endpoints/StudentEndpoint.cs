using MiminalApis.Validators;
using MinimalApis.Extensions.Results;

namespace DevPiraStudents.Endpoints;

public static class StudentEndpoint
{
    public static void MapStudentEndpoint(this WebApplication app)
    {
        app.MapGet("/student", GetAll);
        app.MapGet("/student/{id}", Get);
        app.MapPost("/student", Add).WithValidator<Student>();
    }

    internal static IResult Get(IStudentRepository repository, Guid id)
    {
        var student = repository.Get(id);
        if (student is null)
            return Results.NotFound();
        return Results.Extensions.Ok(student);
    }

    internal static IResult GetAll(IStudentRepository repository)
    {
        return Results.Extensions.Ok(repository.GetAll());
    }

    internal static IResult Add(IStudentRepository repository, Student student)
    {
        repository.Add(student);
        return Results.Extensions.Created($"student/{student.Id}", student);
    }

}
