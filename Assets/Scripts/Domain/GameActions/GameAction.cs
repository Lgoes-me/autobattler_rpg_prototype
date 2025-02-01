using System;
using System.Collections.Generic;
using UnityEngine;

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
        Application.Instance.SceneManager.UseDoorToChangeScene(spawnDomain);
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
public class GivePrize : GameAction
{
    [field: SerializeField] private BlessingIdentifier Blessing { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.BlessingManager.AddBlessing(Blessing);
    }
}