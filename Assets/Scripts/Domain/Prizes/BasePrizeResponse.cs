public class BasePrizeResponse
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

    public void SetWeapon()
    {
        PawnInfo.SetWeapon(Weapon);
    }
}
