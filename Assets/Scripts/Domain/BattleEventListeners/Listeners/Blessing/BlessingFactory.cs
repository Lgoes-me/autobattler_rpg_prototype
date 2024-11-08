using System;

public class BlessingFactory
{
    public Blessing CreateBlessing(BlessingIdentifier id)
    {
        return id switch
        {
            BlessingIdentifier.BattleStartGainMana => new Blessing(id)
            {
                new OnBattleStartedListener()
                {
                    battle => battle.PlayerPawns.ForEach(p => p.Pawn.GainMana())
                }
            },
            
            BlessingIdentifier.OnAttackHeal => new Blessing(id)
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

    private bool IsPlayerTeam(PawnController pawn) => pawn.Pawn.Team == TeamType.Player;
}