using UnityEngine;

public class AbilityResourceComponent
{
    private PawnController AbilityUser { get; set; }
    private int ManaCost { get; set; }

    public AbilityResourceComponent(PawnController abilityUser, int manaCost)
    {
        AbilityUser = abilityUser;
        ManaCost = manaCost;
    }

    public bool HasResource()
    {
        var pawn = AbilityUser.Pawn;
        return pawn.Mana >= ManaCost;
    }
    
    public void SpendResource()
    {
        var pawn = AbilityUser.Pawn;
        
        if(!pawn.HasMana)
            return;

        pawn.Mana = Mathf.Clamp(
            ManaCost > 0 ? pawn.Mana - ManaCost : pawn.Mana + 10, 
            0, 
            pawn.MaxMana);
        
        AbilityUser.PawnCanvasController.UpdateMana();
    }
}