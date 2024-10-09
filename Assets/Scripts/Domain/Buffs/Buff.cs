public abstract class Buff
{
    private PawnDomain DebuffedPawn { get; set; }
    
    public virtual void Init(PawnDomain pawnDomain)
    {
        DebuffedPawn = pawnDomain;
    }
    
    public virtual void Tick()
    {
        
    }

    public virtual void Deactivate()
    {
        
    }
}