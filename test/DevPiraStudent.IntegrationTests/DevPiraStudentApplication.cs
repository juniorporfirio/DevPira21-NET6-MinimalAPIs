using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevPiraStudent.IntegrationTests;

internal class DevPiraStudentApplication : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection> _services;

    public DevPiraStudentApplication(Action<IServiceCollection> services)
    {
        _services = services;
    }
    protected override IHost CreateHost(IHostBuilder builder)
    {
        if(_services is not null)
            builder.ConfigureServices(_services);
            
        return base.CreateHost(builder);
    }
}
