using IQ.Mofy.Configuration.Extensions;
using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using Microsoft.Extensions.Configuration;

namespace IQ.Mofy.Configuration.Steps;

public class AddConfigurationStep : IApplicationPreRunStep
{
    public Task OnPreRunAsync(IApplication application)
    {
        application.AddConfiguration();

        return Task.CompletedTask;
    }
}