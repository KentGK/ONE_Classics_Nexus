namespace ONE_Classics_Nexus.WinForms.Services;

public interface IFormFactory
{
    T CreateForm<T>() where T : Form;
    T CreateForm<T>(params object[] parameters) where T : Form;
}