using System.Collections;
using System.Collections.Generic;

public class Tree<T> : IEnumerable<Tree<T>>
{
    public T Data { get; }
    public List<Tree<T>> ChildrenNodes { get; }
    public Tree<T> Parent { get; private set; }

    public bool IsRoot => Parent == null;
    public bool IsLeaf => ChildrenNodes.Count == 0;

    public Tree(T data)
    {
        Data = data;
        ChildrenNodes = new List<Tree<T>>();
        Parent = null;
    }

    private void SetParent(Tree<T> parent)
    {
        Parent = parent;
    }

    public void Add(T data)
    {
        var tree = new Tree<T>(data);
        tree.SetParent(this);

        ChildrenNodes.Add(tree);
    }

    public void Add(Tree<T> tree)
    {
        tree.SetParent(this);

        ChildrenNodes.Add(tree);
    }

    public IEnumerator<Tree<T>> GetEnumerator()
    {
        yield return this;

        foreach (var directChild in ChildrenNodes)
        {
            foreach (var anyChild in directChild)

                yield return anyChild;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}