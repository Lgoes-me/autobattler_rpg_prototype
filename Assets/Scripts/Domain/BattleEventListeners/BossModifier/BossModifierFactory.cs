using System;

public class BossModifierFactory
{
    public BossModifier CreateBossModifier(BossModifierIdentifier id, Rarity rarity)
    {
        return id switch
        {
            BossModifierIdentifier.Unknown => new BossModifier(id, rarity)
            {
            },
            
            BossModifierIdentifier.InimigosSeCuramAoAtacar => new BossModifier(id, rarity)
            {
                new OnAttackEventListener()
                {
                    (_, abilityUser, _) => IsEnemyTeam(abilityUser),
                    (_, abilityUser, _, rarity) =>
                    {
                        var healValue = rarity switch
                        {
                            Rarity.Deactivated => 0,
                            Rarity.Legendary => 20,
                            Rarity.Epic => 15,
                            Rarity.Rare => 10,
                            Rarity.Uncommon => 5,
                            Rarity.Common => 2,
                            _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
                        };

                        abilityUser.Pawn.GetComponent<ResourceComponent>().ReceiveHeal(healValue, false);
                    }
                }
            },
            
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };
    }
    
    private bool IsEnemyTeam(PawnController pawn) => pawn.Pawn.Team == TeamType.Enemies;
}