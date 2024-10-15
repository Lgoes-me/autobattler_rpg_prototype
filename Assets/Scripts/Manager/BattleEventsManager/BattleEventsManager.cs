using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleEventsManager : MonoBehaviour
{
    public List<JokerIdentifier> JokersReadOnly => Jokers.Select(j => j.Identifier).ToList();
    private List<Joker> Jokers { get; set; }
    private JokerFactory JokerFactory { get; set; }

    public void Init()
    {
        JokerFactory = new JokerFactory();
        Jokers = Application.Instance.Save.Jokers.Select(j => JokerFactory.CreateJoker(j)).ToList();
    }

    public void AddJoker(JokerIdentifier jokerIdentifier)
    {
        Jokers.Add(JokerFactory.CreateJoker(jokerIdentifier));
        SaveOperation();
    }


    public void RemoveJoker(JokerIdentifier jokerIdentifier)
    {
        var jokerToRemove = Jokers.FirstOrDefault(j => j.Identifier == jokerIdentifier);
        if (jokerToRemove == null)
            return;

        Jokers.Remove(jokerToRemove);
        SaveOperation();
    }

    public void ReorderJokers(List<JokerIdentifier> jokerIdentifiers)
    {
        Jokers = Application.Instance.Save.Jokers.Select(j => JokerFactory.CreateJoker(j)).ToList();
        SaveOperation();
    }

    private void SaveOperation()
    {
        var save = Application.Instance.Save;
        save.Jokers = Jokers.Select(j => j.Identifier).ToList();
        Application.Instance.SaveManager.SaveData(save);
    }
    
    public void DoBattleStartEvent(Battle battle)
    {
        foreach (Joker joker in Jokers)
        {
            foreach (BaseBattleEventListener listener in joker)
            {
                if (listener is not OnBattleStartedListener onBattleStartedListener)
                    continue;

                onBattleStartedListener.OnBattleStarted(battle);
            }
        }
    }
}