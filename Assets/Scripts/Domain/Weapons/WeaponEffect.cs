using System.Collections.Generic;

public class WeaponEffect
{
    public EffectType Type { get; }
    public List<AbilityBehaviourData> Effects { get; }
    
    public WeaponEffect(EffectType type, List<AbilityBehaviourData> effects)
    {
        Type = type;
        Effects = effects;
    }
}