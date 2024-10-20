using System;

public class ArchetypeFactory
{
    public Archetype CreateArchetype(ArchetypeIdentifier id, int quantidade)
    {
        return id switch
        {
            ArchetypeIdentifier.Teste => new Archetype(id)
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