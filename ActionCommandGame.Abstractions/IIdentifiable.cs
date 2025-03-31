namespace ActionCommandGame.Abstractions
{
    public interface IIdentifiable<T>
    {
        T Id { get; set; }
    }

    public interface IIdentifiable: IIdentifiable<int>
    {
    }
}
