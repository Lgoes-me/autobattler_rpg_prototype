using UnityEngine;

[System.Serializable]
public abstract class Buff
{
    public string Id { get; private set; }
    protected Pawn Pawn { get; private set; }

    protected float Duration { get; set; }
    private float StartingTime { get; set; }
    
    public int Priority { get; set; }
    public string CharacterInfoIdentifier { get; set; }

    protected Buff(string id, float duration)
    {
        Id = id;
        Duration = duration;
        StartingTime = Time.time;
    }

    public virtual void Init(Pawn pawn)
    {
        Pawn = pawn;
    }

    public virtual void Tick()
    {
        if (Duration < 0 || Time.time - StartingTime < Duration)
            return;

        RemoveSelf();
    }

    public virtual void Deactivate()
    {
    }

    public virtual void TryReapplyBuff()
    {
    }
    
    private void RemoveSelf()
    {
        Pawn.RemoveBuff(this);
    }
}