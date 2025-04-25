
public abstract class BasePawnCanvasController: BaseCanvasController
{
    protected Pawn Pawn { get; private set; }
    
    public virtual void Init(Pawn pawn)
    {
        Pawn = pawn;
        
        Pawn.GetComponent<StatsComponent>().BattleStarted += StartBattle;
        Pawn.GetComponent<StatsComponent>().BattleFinished += FinishBattle;
    }

    protected virtual void StartBattle()
    {
        Show();
    }

    protected virtual void FinishBattle()
    {
        Hide();
    }
    
    protected virtual void Death()
    {
        Hide();
    }
    
    protected virtual void OnDestroy()
    {
        if(Pawn == null)
            return;
        
        Pawn.GetComponent<StatsComponent>().BattleStarted -= StartBattle;
        Pawn.GetComponent<StatsComponent>().BattleFinished -= FinishBattle;
    }
}