public abstract class Buff
{
    protected PawnDomain DebuffedPawn { get; private set; }
    
    public virtual void Init(PawnDomain pawnDomain)
    {
        DebuffedPawn = pawnDomain;
    }
    
    public virtual void Tick()
    {
        
    }

    public virtual void Deactivate()
    {
        DebuffedPawn.RemoveBuff(this);
    }
}