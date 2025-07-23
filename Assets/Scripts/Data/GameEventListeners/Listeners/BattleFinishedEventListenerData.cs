using System;
using UnityEngine;

[Serializable]
public class BattleFinishedEventListenerData : BaseEvent
{
    [field: SerializeField] [field: SerializeReference] protected IBattleEffect Effect { get; set; }
    
    public void OnBattleFinished(Battle battle)
    {
        Effect.OnBattleStateChanged(battle);
    }
}
