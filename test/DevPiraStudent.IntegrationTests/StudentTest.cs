using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace DevPiraStudent.IntegrationTests;

public class StudentTest
{
    private readonly IStudentRepository repository;

    public StudentTest()
    {
        repository = Substitute.For<IStudentRepository>();
    }
    [Fact]
    public async Task Ao_acessar_o_endpoint_para_selecionar_todos_os_estudantes_deve_retornar_dados()
    {
        //Arrange
        var studentMock = new List<Student>
        {
            new Student { Id=System.Guid.NewGuid(), FirstName="Junior", LastName="Porfirio"}
        };

        repository.GetAll().Returns(studentMock);

        var application = new DevPiraStudentApplication(services =>
         {
             services.AddScoped(_ => repository);
         });

        using var client = application.CreateClient();

        //Act
        var request = await client.GetAsync("/student");
        var response = await request.Content.ReadAsStringAsync();
        var students = JsonSerializer.Deserialize<IEnumerable<Student>>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //Assert
        request.StatusCode.Should().Be(HttpStatusCode.OK);
        students.Should().BeEquivalentTo(studentMock);

    }

    [Fact]
    public async Task Ao_acessar_o_endpoint_para_selectionar_o_estudante_por_Id_deve_retornar_apenas_um_estudante()
    {
        //Arrange
        var code = System.Guid.NewGuid();
        var studentMock = new Student { Id = code, FirstName = "Junior", LastName = "Porfirio" };
        repository.Get(code).Returns(studentMock);

        var application = new DevPiraStudentApplication(services =>
         {
             services.AddScoped(_ => repository);
         });

        using var client = application.CreateClient();

        //Act
        var request = await client.GetAsync($"/student/{code}"); ;
        var response = await request.Content.ReadAsStringAsync();
        var student = JsonSerializer.Deserialize<Student>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //Assert
        request.StatusCode.Should().Be(HttpStatusCode.OK);
        student.Should().BeEquivalentTo(studentMock);

    }


    [Fact]
    public async Task Ao_acessar_o_endpoint_para_adicionar_um_estudante_deve_retornar_o_estudante_adicionado()
    {
        //Arrange
        var code = System.Guid.NewGuid();
        var studentMock = new Student { Id = code, FirstName = "Junior", LastName = "Porfirio" };
        repository.Add(studentMock);

        var application = new DevPiraStudentApplication(services =>
         {
             services.AddScoped(_ => repository);
         });

        using var client = application.CreateClient();

        //Act
        var request = await client.PostAsync($"/student", JsonContent.Create(studentMock));
        var response = await request.Content.ReadAsStringAsync();
        var student = JsonSerializer.Deserialize<Student>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        //Assert
        repository.Received().Add(studentMock);
        request.StatusCode.Should().Be(HttpStatusCode.Created);
        student.Should().BeEquivalentTo(studentMock);

    }


}