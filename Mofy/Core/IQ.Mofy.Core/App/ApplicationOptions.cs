using IQ.Mofy.Core.Abstractions.App;

namespace IQ.Mofy.Core.App;

public class ApplicationOptions : IApplicationOptions
{
    #region IApplicationOptions
    
    public bool ValidateScopes { get; set; } = true;
    
    public bool ValidateOnBuild { get; set; }

    #endregion
}