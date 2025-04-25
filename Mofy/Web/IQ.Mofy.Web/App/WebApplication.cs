using IQ.Mofy.Core.App;

namespace IQ.Mofy.Web.App;

public abstract class WebApplication(IServiceCollection serviceCollection) : Application(serviceCollection)
{
    protected WebApplication() : this(new ServiceCollection()) { }
}