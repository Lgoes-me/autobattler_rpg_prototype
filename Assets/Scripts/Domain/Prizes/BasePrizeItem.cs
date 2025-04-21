
using UnityEngine;

public class BasePrizeItem
{
    public string Name { get; protected set; }
    public Sprite Image { get; }
    public string Description { get; }
}

public class PawnPrizeItem : BasePrizeItem
{
    public PawnInfo PawnInfo { get; }

    public PawnPrizeItem(PawnInfo pawnInfo)
    {
        Name = pawnInfo.Name;
        PawnInfo = pawnInfo;
    }
}

public class BlessingPrizeItem : BasePrizeItem
{
    public BlessingIdentifier Blessing { get; }

    public BlessingPrizeItem(BlessingIdentifier blessing)
    {
        Name = blessing.ToString();
        Blessing = blessing;
    }
}

public class WeaponPrizeItem : BasePrizeItem
{
    public PawnInfo PawnInfo { get; }
    private WeaponData Weapon { get; }

    public WeaponPrizeItem(PawnInfo pawnInfo, WeaponData weapon)
    {
        Name = $"{weapon.Id} to {pawnInfo.Name}";
        PawnInfo = pawnInfo;
        Weapon = weapon;
    }

    public void ApplyPrize()
    {
        PawnInfo.SetWeapon(Weapon);
    }
}

public class AbilityPrizeItem : BasePrizeItem
{
    public PawnInfo PawnInfo { get; }
    private AbilityData Ability { get; }

    public AbilityPrizeItem(PawnInfo pawnInfo, AbilityData ability)
    {
        Name = $"{ability.Id} to {pawnInfo.Name}";
        PawnInfo = pawnInfo;
        Ability = ability;
    }

    public void ApplyPrize()
    {
        PawnInfo.SetAbility(Ability);
    }
}

public class BuffPrizeItem : BasePrizeItem
{
    public PawnInfo PawnInfo { get; }
    private BuffData Buff { get; }

    public BuffPrizeItem(PawnInfo pawnInfo, BuffData buff)
    {
        Name = $"{buff.Id} to {pawnInfo.Name}";
        PawnInfo = pawnInfo;
        Buff = buff;
    }

    public void ApplyPrize()
    {
        PawnInfo.SetBuff(Buff);
    }
}

public class ConsumablePrizeItem : BasePrizeItem
{
    public ConsumableData Consumable { get; }

    public ConsumablePrizeItem(ConsumableData consumable)
    {
        Name = consumable.Id;
        Consumable = consumable;
    }
}