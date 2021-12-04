using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DevPiraStudents_NET5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("/student")]
        public IActionResult GetAll([FromServices] IStudentRepository repository)
        {
            return Ok(repository.GetAll());
        }

        [HttpGet("/student/{Id:Guid}")]
        public IActionResult Get([FromServices] IStudentRepository repository, Guid Id)
        {
            var student = repository.Get(Id);

            if (student is null) return NotFound();

            return Ok(student);
        }

        [HttpPost("/student")]
        public IActionResult AddStudent([FromServices] IStudentRepository repository, Student student)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            repository.Add(student);    

            return Created($"student/{student.Id}", student);
        }
    }

   public record Student()
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
}
