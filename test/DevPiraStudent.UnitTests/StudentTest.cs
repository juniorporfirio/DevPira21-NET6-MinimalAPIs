using System.Collections.Generic;
using DevPiraStudents.Endpoints;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DevPiraStudent.UnitTests;

public class StudentTest
{
    private readonly IStudentRepository repository;

    public StudentTest() => repository = Substitute.For<IStudentRepository>();
    
    [Fact]
    public void Ao_adicionar_um_novo_estudante_deve_retornar_o_proprio_estudante_inserido()
    {
        //Arrange
        var studentMoq = new Student{ Id= System.Guid.NewGuid(), FirstName = "Junior", LastName = "Porfirio" };
        repository.Add(studentMoq);

        //Act
        var student =  StudentEndpoint.Add(repository, studentMoq);

        //Assert
        var result = student.Should().BeOfType<MinimalApis.Extensions.Results.Created<Student>>().Subject;
        repository.Received().Add(studentMoq);
        result.StatusCode.Should().Be(201);
        result.Value.Should().BeEquivalentTo(studentMoq);

    }


    [Fact]
    public void Ao_selecionar_um_estudante_por_codigo_deve_retornar_o_estudante()
    {
        //Arrange
        var studentMoq = new Student{ Id= System.Guid.NewGuid(), FirstName = "Junior", LastName = "Porfirio" };
        repository.Get(studentMoq.Id).Returns(studentMoq);

        //Act
        var student =  StudentEndpoint.Get(repository, studentMoq.Id);

        //Assert
        var result = student.Should().BeOfType<MinimalApis.Extensions.Results.Ok<Student>>().Subject;
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(studentMoq);

    }

   [Fact]
    public void Ao_selecionar_todos_estudantes__deve_retornar_todos()
    {
        //Arrange
        var studentsMoq = new List<Student> {
            new Student{ Id= System.Guid.NewGuid(), FirstName = "Junior", LastName = "Porfirio" }
        };

        repository.GetAll().Returns(studentsMoq);

        //Act
        var student =  StudentEndpoint.GetAll(repository);

        //Assert
        var result = student.Should().BeOfType<MinimalApis.Extensions.Results.Ok<IEnumerable<Student>>>().Subject;
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(studentsMoq);

    }

}