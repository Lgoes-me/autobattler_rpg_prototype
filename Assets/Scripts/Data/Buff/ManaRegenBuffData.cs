using UnityEngine;

[System.Serializable]
public class ManaRegenBuffData : BuffComponentData
{
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int TickRate { get; set; }

    public override BuffComponent ToDomain(Pawn pawn)
    {
        return new ManaRegenBuff(Regen, TickRate);
    } 
}

[System.Serializable]
public class ManaRegenNegativoBuffData : BuffComponentData
{
    [field: SerializeField] private int Regen { get; set; }
    [field: SerializeField] private int TickRate { get; set; }

    public override BuffComponent ToDomain(Pawn pawn)
    {
        return new ManaRegenNegativoBuff(Regen, TickRate);
    } 
}