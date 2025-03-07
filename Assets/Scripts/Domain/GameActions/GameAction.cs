using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public abstract class GameAction : IComponentData
{
    public abstract void Invoke();
}

[Serializable]
public class OpenCutscene : GameAction
{
    [field: SerializeField] private string SceneName { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.SceneManager.OpenCutscene(SceneName);
    }
}

[Serializable]
public class ChangeScene : GameAction
{
    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] private string SpawnId { get; set; }
    
    public override void Invoke()
    {
        var spawnDomain = new SpawnDomain(SpawnId, SceneName);
        Application.Instance.SceneManager.ChangeContext(spawnDomain);
    }
}

[Serializable]
public class AddFriend : GameAction
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.PartyManager.AddToAvailableParty(PawnData);
    }
}

[Serializable]
public class MultipleActions : GameAction
{
    [field: SerializeReference] [field: SerializeField] private List<GameAction> GameActions { get; set; }
    
    public override void Invoke()
    {
        foreach (var gameAction in GameActions)
        {
            gameAction.Invoke();
        }
    }
}

[Serializable]
public class GiveBlessing : GameAction
{
    [field: SerializeField] private BlessingIdentifier Blessing { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.BlessingManager.AddBlessing(Blessing);
    }
}

[Serializable]
public class GiveRandomBlessing : GameAction
{
    public override void Invoke()
    {
        var listOfBlessings = Enum.GetValues(typeof(BlessingIdentifier)).Cast<BlessingIdentifier>().ToList();
        var randomBlessing = listOfBlessings[Random.Range(0, listOfBlessings.Count)];
        Application.Instance.BlessingManager.AddBlessing(randomBlessing);
    }
}


[Serializable]
public class GiveRandomBlessingFromPool : GameAction
{
    [field: SerializeField] private List<BlessingIdentifier> Pool { get; set; }
    
    public override void Invoke()
    {
        var randomBlessing = Pool[Random.Range(0, Pool.Count)];
        Application.Instance.BlessingManager.AddBlessing(randomBlessing);
    }
}