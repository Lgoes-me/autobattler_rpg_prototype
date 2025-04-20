using UnityEngine;

[System.Serializable]
public class RegenBuffData : BuffComponentData
{
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int TickRate { get; set; }

    public override BuffComponent ToDomain(Pawn pawn)
    {
        var regen = Regen;// * abilityUser.Pawn.Stats.Strength;
        return new RegenBuff(regen, TickRate);
    }
}