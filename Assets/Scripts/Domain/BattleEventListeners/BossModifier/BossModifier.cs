using System;
using UnityEngine;

[Serializable]
public class BossModifierData
{
    [field: SerializeField] private BossModifierIdentifier Identifier { get; set; }
    [field: SerializeField] private Rarity Rarity { get; set; }

    public BossModifier ToDomain(BossModifierFactory factory)
    {
        return factory.CreateBossModifier(Identifier, Rarity);
    }
}

public class BossModifier : BaseIEnumerableGameEventListener
{
    public BossModifierIdentifier Identifier { get; }

    public BossModifier(BossModifierIdentifier identifier, Rarity rarity)
    {
        Identifier = identifier;
        Rarity = rarity;
    }
}

public enum BossModifierIdentifier
{
    Unknown = 0,
    
    InimigosSeCuramAoAtacar = 01_01,
}