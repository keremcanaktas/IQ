using IQ.Mofy.Core.App;
using Application = System.Windows.Forms.Application;

namespace IQ.WindowsApplication.Test;

static class Program
{
    [STAThread]
    static async Task Main()
    {
        var application = new Mofy.Core.App.Application();

        application.ServiceCollection.AddRegify();

        await application.RunAsync();

        Application.Run(new Form1());
    }
}