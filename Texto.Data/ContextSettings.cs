namespace Texto.Data
{
    public interface IContextSettings
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        string CollectionName { get; }
    }
}
