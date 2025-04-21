using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.App;

public interface IApplicationOptions
{
    public bool ValidateScopes { get; set; }

    public bool ValidateOnBuild { get; set; }
}