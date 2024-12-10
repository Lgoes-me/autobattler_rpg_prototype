using System;
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
        Application.Instance.SceneManager.UseDoorToChangeScene(SpawnId, SceneName);
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