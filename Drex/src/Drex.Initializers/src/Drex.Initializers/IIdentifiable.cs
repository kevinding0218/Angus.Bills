namespace Drex.Initializers
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
    }
}