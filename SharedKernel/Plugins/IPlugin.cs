namespace SharedKernel.Plugins
{
    public interface IPlugin
    {
        bool Install();
        bool Uninstall();
        bool Load();
    }
}
