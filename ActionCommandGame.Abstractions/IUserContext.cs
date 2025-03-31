namespace ActionCommandGame.Abstractions
{
    public interface IUserContext<T>
    {
        T UserId { get; set; }
    }

    public interface IUserContext : IUserContext<int>
    {

    }
}
