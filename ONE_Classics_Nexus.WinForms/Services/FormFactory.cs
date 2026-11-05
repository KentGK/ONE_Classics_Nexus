using Microsoft.Extensions.DependencyInjection;

namespace ONE_Classics_Nexus.WinForms.Services;

public sealed class FormFactory : IFormFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FormFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public T CreateForm<T>() where T : Form
    {
        return ActivatorUtilities.CreateInstance<T>(_serviceProvider);
    }

    public T CreateForm<T>(params object[] parameters) where T : Form
    {
        return ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);
    }
}