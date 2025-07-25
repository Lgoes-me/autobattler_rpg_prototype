﻿using System.Collections.Generic;

public class AbilityBehaviour
{
    private List<AbilityEffect> Effects { get; set; }
    private AbilityFocusComponent Focus { get; set; }
    public AbilityActionComponent Action { get; private set; }
    
    public AbilityBehaviour(
        List<AbilityEffect> effects,
        AbilityFocusComponent focus,
        AbilityActionComponent action)
    {
        Effects = effects;
        Focus = focus;
        Action = action;
    }
    
    public void ChooseFocus(PawnController pawn, Battle battle)
    {
        Focus?.ChooseFocus(pawn, battle);
    }
    
    public void DoAction()
    {
        Action.DoAction();
    }
}