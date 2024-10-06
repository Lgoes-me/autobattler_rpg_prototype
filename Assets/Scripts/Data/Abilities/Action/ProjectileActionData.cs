using UnityEngine;

[System.Serializable]
public class ProjectileActionData : BaseActionData
{
    [field: SerializeField] private ProjectileController Projectile { get; set; }

    public override AbilityActionComponent ToDomain(
        PawnController abilityUser,
        AbilityFocusComponent focusComponent,
        AbilityEffect effect)
    {
        return new ProjectileActionComponent(abilityUser, focusComponent, effect, Projectile);
    }
}