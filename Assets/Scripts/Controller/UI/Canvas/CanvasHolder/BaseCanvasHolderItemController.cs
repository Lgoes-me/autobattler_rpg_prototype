public abstract class BaseCanvasHolderItemController<T> : BaseCanvasController
{
    public T Data { get; protected set; }
    public abstract BaseCanvasHolderItemController<T> Init(T data);
}