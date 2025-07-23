using System;
using UnityEngine;

[Serializable]
public class OpcoesExtrasEffectData : IBlessingCreatedEffect
{
    [field: SerializeField] private int OpcoesExtras { get; set; }

    public void OnBlessingCreated() => DoEffect();

    private void DoEffect()
    {
        Application.Instance.GetManager<PlayerManager>().PlayerStats.StatsDictionary
            .Add(Stat.OpcoesExtras, OpcoesExtras);
    }
}