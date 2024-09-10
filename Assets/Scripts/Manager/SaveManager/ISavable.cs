public interface ISavable<in T>
{
    string Id { get; }
    void SaveData(T data);
}
