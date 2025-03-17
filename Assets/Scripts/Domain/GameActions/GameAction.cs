﻿using System;
using System.Collections.Generic;
using System.Linq;
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
public class OpenBlessingPrize : GameAction
{
    public override void Invoke()
    {
        var blessings = Enum.GetValues(typeof(BlessingIdentifier)).Cast<BlessingIdentifier>().ToList();
        Application.Instance.PrizeManager.CreateBlessingPrize(blessings);
    }
}

[Serializable]
public class OpenBlessingPrizeFromPool : GameAction
{
    [field: SerializeField] private List<BlessingIdentifier> Pool { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.PrizeManager.CreateBlessingPrize(Pool);
    }
}

[Serializable]
public class OpenLevelUpPrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.PrizeManager.CreateLevelUpPrize();
    }
}
