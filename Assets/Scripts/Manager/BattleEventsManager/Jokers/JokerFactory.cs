using System;

public class JokerFactory
{
    public Joker CreateJoker(JokerIdentifier id)
    {
        return id switch
        {
            JokerIdentifier.BattleStartGainMana => new Joker(id)
            {
                new OnBattleStartedListener()
                {
                    battle => battle.PlayerPawns.ForEach(p =>
                    {
                       p.Pawn.GainMana(); 
                       p.PawnCanvasController.UpdateMana();
                    })
                }
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
}

public enum JokerIdentifier
{
    BattleStartGainMana,
}