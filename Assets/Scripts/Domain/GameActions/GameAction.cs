using System;
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
        Application.Instance.GetManager<SceneManager>().OpenCutscene(SceneName);
    }
}

[Serializable]
public class ChangeScene : GameAction
{
    [field: SerializeField] private string SceneName { get; set; }
    [field: SerializeField] private string SpawnId { get; set; }
    
    public override void Invoke()
    {
        var spawnDomain = new Spawn(SpawnId, SceneName);
        Application.Instance.GetManager<SceneManager>().ChangeContext(spawnDomain);
    }
}

[Serializable]
public class AddFriend : GameAction
{
    [field: SerializeField] private PawnData PawnData { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.GetManager<PartyManager>().AddToAvailableParty(PawnData);
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
        Application.Instance.GetManager<PrizeManager>().CreateBlessingPrize(blessings);
    }
}

[Serializable]
public class OpenBlessingPrizeFromPool : GameAction
{
    [field: SerializeField] private List<BlessingIdentifier> Pool { get; set; }
    
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreateBlessingPrize(Pool);
    }
}

[Serializable]
public class OpenLevelUpPrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreateLevelUpPrize();
    }
}

[Serializable]
public class OpenPartyMemberPrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreatePartyMemberPrize();
    }
}

[Serializable]
public class OpenWeaponPrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreateWeaponPrize();
    }
}

[Serializable]
public class OpenAbilityPrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreateAbilityPrize();
    }
}

[Serializable]
public class OpenBuffPrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreateBuffPrize();
    }
}

[Serializable]
public class OpenConsumablePrize : GameAction
{
    public override void Invoke()
    {
        Application.Instance.GetManager<PrizeManager>().CreateConsumablePrize();
    }
}