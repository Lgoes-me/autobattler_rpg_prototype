﻿using System.Collections.Generic;

public class AbilityBehaviour
{
    private List<AbilityEffect> Effects { get; set; }
    private AbilityFocusComponent Focus { get; set; }
    private AbilityActionComponent Action { get; set; }
    
    public AbilityBehaviour(
        List<AbilityEffect> effects,
        AbilityFocusComponent focus,
        AbilityActionComponent action)
    {
        Effects = effects;
        Focus = focus;
        Action = action;
    }
    
    public PawnController ChooseFocus(Battle battle)
    {
        return Focus.ChooseFocus(battle);
    }
    
    public void DoAction()
    {
        Action.DoAction();
    }
}