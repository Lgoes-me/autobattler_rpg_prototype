using UnityEngine;

[System.Serializable]
public class RegenBuffData : BuffComponentData
{
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int TickRate { get; set; }
    [field: SerializeField]  private float Duration { get; set; }

    public override Buff ToDomain(string id, PawnController abilityUser)
    {
        var regen = Regen;// * abilityUser.Pawn.Stats.Strength;
        return new RegenBuff(regen, TickRate, id, Duration);
    }
}