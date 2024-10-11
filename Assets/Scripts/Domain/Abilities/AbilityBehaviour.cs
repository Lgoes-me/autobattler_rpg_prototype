public class AbilityBehaviour
{
    private AbilityEffect Effect { get; set; }
    private AbilityFocusComponent Focus { get; set; }
    private AbilityActionComponent Action { get; set; }
    
    public AbilityBehaviour(
        AbilityEffect effect,
        AbilityFocusComponent focus,
        AbilityActionComponent action)
    {
        Effect = effect;
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