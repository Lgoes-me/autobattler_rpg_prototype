﻿public class BasePrizeResponse
{
        
}

public class WeaponPrizeResponse : BasePrizeResponse
{
    public PawnInfo PawnInfo { get; private set; }
    public WeaponData Weapon { get; private set; }

    public WeaponPrizeResponse(PawnInfo pawnInfo, WeaponData weapon)
    {
        PawnInfo = pawnInfo;
        Weapon = weapon;
    }

    public void ApplyPrize()
    {
        PawnInfo.SetWeapon(Weapon);
    }
}


public class AbilityPrizeResponse : BasePrizeResponse
{
    public PawnInfo PawnInfo { get; private set; }
    public AbilityData Ability { get; private set; }

    public AbilityPrizeResponse(PawnInfo pawnInfo, AbilityData ability)
    {
        PawnInfo = pawnInfo;
        Ability = ability;
    }

    public void ApplyPrize()
    {
        PawnInfo.SetAbility(Ability);
    }
}
