using IQ.Mofy.Regify.Extensions;

namespace IQ.WindowsApplication.Test;

static class Program
{
    [STAThread]
    static async Task Main()
    {
        var application = new Mofy.Core.App.Application();

        application.UseRegify();

        await application.RunAsync();
        
        Application.Run(new Form1());
    }
}