using System.Collections;
using System.Collections.Generic;

public class Joker : IEnumerable
{
    public JokerIdentifier Identifier { get; set; }
    private List<BaseBattleEventListener> GameEventListeners { get; set; }

    public Joker(JokerIdentifier identifier)
    {
        Identifier = identifier;
        GameEventListeners = new List<BaseBattleEventListener>();
    }

    public void Add(BaseBattleEventListener item)
    {
        GameEventListeners.Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GameEventListeners.GetEnumerator();
    }
}