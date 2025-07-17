using UnityEngine;

[System.Serializable]
public class RegenBuffData : BuffComponentData
{
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int TickRate { get; set; }

    public override BuffComponent ToDomain(Pawn pawn)
    {
        return new RegenBuff(Regen, TickRate);
    }
}