
public abstract class BasePawnCanvasController: BaseCanvasController
{
    protected Pawn Pawn { get; private set; }
    
    public virtual void Init(Pawn pawn)
    {
        Pawn = pawn;
        
        Pawn.BattleStarted += StartBattle;
        Pawn.BattleFinished += FinishBattle;
    }

    protected virtual void StartBattle(Battle battle)
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
    
    private void OnDestroy()
    {
        if (Pawn == null)
            return;
        
        Terminate();
    }

    protected virtual void Terminate()
    {
        Pawn.BattleStarted -= StartBattle;
        Pawn.BattleFinished -= FinishBattle;
    }
}