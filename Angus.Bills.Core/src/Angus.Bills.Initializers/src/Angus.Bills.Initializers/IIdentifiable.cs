namespace Angus.Bills.Initializers {
    public interface IIdentifiable<out T> {
        T Id { get; }
    }
}