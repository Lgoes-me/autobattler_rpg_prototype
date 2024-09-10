public interface ILoadable<T>
{
    string Id { get; set; }
    T LoadData(T container);
}