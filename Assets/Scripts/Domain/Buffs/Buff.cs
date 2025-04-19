using UnityEngine;

[System.Serializable]
public abstract class Buff
{
    public string Id { get; private set; }
    protected float Duration { get; set; }
    
    protected PawnController PawnController { get; private set; }
    
    private float StartingTime { get; set; }
    
    public int Priority { get; set; }
    public string CharacterInfoIdentifier { get; set; }

    protected Buff(string id, float duration)
    {
        Id = id;
        Duration = duration;
        StartingTime = Time.time;
    }

    public virtual void Init(PawnController pawnController)
    {
        PawnController = pawnController;
        PawnController.ReceiveBuff(this);
    }

    public virtual bool Tick()
    {
        if (Duration < 0 || Time.time - StartingTime < Duration)
            return false;

        return true;
    }

    public virtual void Deactivate()
    {
    }

    public virtual void TryReapplyBuff()
    {
        
    }
}