using System;
using UnityEngine;

[Serializable]
public class BlessingCreatedEventListenerData : BaseBattleEventListenerData
{
    [field: SerializeField] [field: SerializeReference] private IBlessingCreatedEffect Effect { get; set; }

    public void OnBlessingCreated() => Effect.OnBlessingCreated();
}

public interface IBlessingCreatedEffect : IBlessingEffectData
{
    void OnBlessingCreated();
}