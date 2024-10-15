using System.Collections;
using System.Collections.Generic;

public abstract class BaseBattleEventListener
{
    
}

public abstract class BaseBattleEventListener<T, T1>: BaseBattleEventListener, IEnumerable
{
    protected List<T> Validators { get; set; }
    protected List<T1> Modifiers { get; set; }

    protected BaseBattleEventListener()
    {
        Validators = new List<T>();
        Modifiers = new List<T1>();
    }

    public void Add(T validator)
    {
        Validators.Add(validator);
    }
    
    public void Add(T1 modifier)
    {
        Modifiers.Add(modifier);
    }

    public IEnumerator GetEnumerator()
    {
        return Modifiers.GetEnumerator();
    }
}