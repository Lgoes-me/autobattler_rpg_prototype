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
            
            JokerIdentifier.OnAttackHeal => new Joker(id)
            {
                new OnAttackEventListener()
                {
                    (abilityUser, ability) => IsPlayerTeam(abilityUser),
                    (abilityUser, ability) =>
                    {
                        abilityUser.Pawn.ReceiveHeal(5, false);
                        abilityUser.ReceiveHeal(false);
                    }
                }
            },

            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }

    private bool IsPlayerTeam(PawnController pawn) => pawn.Team == TeamType.Player;
}

public enum JokerIdentifier
{
    BattleStartGainMana,
    OnAttackHeal,
}