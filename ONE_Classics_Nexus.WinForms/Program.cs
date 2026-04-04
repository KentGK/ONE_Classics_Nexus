using Microsoft.Extensions.DependencyInjection;
using ONE_Classics_Nexus.WinForms.Forms;
using ONE_Classics_Nexus.WinForms.Services;

namespace ONE_Classics_Nexus.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

        var services = new ServiceCollection();

        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5001/")
        });

        services.AddSingleton<ApiClient>();
        services.AddSingleton<MainForm>();

        using var serviceProvider = services.BuildServiceProvider();

        var mainForm = serviceProvider.GetRequiredService<MainForm>();
        System.Windows.Forms.Application.Run(mainForm);
    }
}