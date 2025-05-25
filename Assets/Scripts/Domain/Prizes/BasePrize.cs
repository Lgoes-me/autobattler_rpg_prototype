using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class BasePrize<T> where T : BasePrizeItem
{
    public List<T> Options { get; internal set; }
}

public class LevelUpPrize : BasePrize<PawnPrizeItem>
{
    public LevelUpPrize(int numberOfOptions, List<PawnInfo> selectedParty)
    {
        var partyOrderedByLevel =
            selectedParty.Where(p => p.CanLevelUp()).OrderByDescending(p => p.Level).ToList();

        Options =
            partyOrderedByLevel.Where(p => p.Level == partyOrderedByLevel[0].Level).OrderBy(_ => Guid.NewGuid())
                .Take(numberOfOptions).Select(p => new PawnPrizeItem(p)).ToList();
    }
}

public class BlessingPrize : BasePrize<BlessingPrizeItem>
{
    public BlessingPrize(int numberOfOptions, List<BlessingIdentifier> blessings)
    {
        Options =
            blessings
                .OrderBy(_ => Guid.NewGuid())
                .Take(numberOfOptions)
                .Select(b => new BlessingPrizeItem(b)).ToList();
    }
}

public class PartyMemberPrize : BasePrize<PawnPrizeItem>
{
    public PartyMemberPrize(int numberOfOptions, int level, List<PawnController> pawns, ContentManager contentManager)
    {
        var ids = pawns.Select(p => p.Pawn.Id).ToList();

        Options =
            contentManager.AvailablePawns
                .Where(p => !ids.Contains(p.Id))
                .OrderBy(_ => Guid.NewGuid())
                .Take(numberOfOptions)
                .Select(p =>
                {
                    var pawnInfo = new PawnInfo(
                        p.Id,
                        level,
                        0,
                        PawnStatus.Transient,
                        p.GetComponent<WeaponComponent>().Weapon,
                        p.GetComponent<AbilitiesComponent>().Abilities,
                        p.GetComponent<PawnBuffsComponent>().PermanentBuffs,
                        p.GetComponent<ConsumableComponent>().Consumables);

                    return new PawnPrizeItem(pawnInfo);
                })
                .ToList();
    }
}

public class WeaponPrize : BasePrize<WeaponPrizeItem>
{
    public WeaponPrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomWeapons =
            contentManager.AvailableWeapons
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

        var partyOrderedByWeaponLevel =
            selectedParty.OrderByDescending(p =>
            {
                if (string.IsNullOrEmpty(p.Weapon))
                    return 0;

                return contentManager.GetWeaponFromId(p.Weapon).Level;
            }).ToList();

        var pawns = partyOrderedByWeaponLevel.Where(p => p.Level == partyOrderedByWeaponLevel[0].Level)
            .OrderBy(_ => Guid.NewGuid()).Take(numberOfOptions).ToList();

        var options = new List<WeaponPrizeItem>();

        foreach (var pawnInfo in pawns)
        {
            var pawn = contentManager.GetPawnFromId(pawnInfo.Name);
            var weapon = randomWeapons.First(w =>
                    pawn.GetComponent<WeaponComponent>().WeaponType.IsEnumFlagPresent(w.Type) &&
                    pawnInfo.Weapon != w.Id)
                .ToDomain();

            options.Add(new WeaponPrizeItem(pawnInfo, weapon));
        }

        Options = options;
    }
}

public class AbilityPrize : BasePrize<AbilityPrizeItem>
{
    public AbilityPrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomAbilities =
            contentManager.AvailableAbilities
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

        var pawns = selectedParty.OrderBy(_ => Guid.NewGuid()).Take(numberOfOptions).ToList();

        var options = new List<AbilityPrizeItem>();

        foreach (var pawnInfo in pawns)
        {
            var pawn = contentManager.GetPawnFromId(pawnInfo.Name);
            var ability = randomAbilities.FirstOrDefault(a =>
                pawn.GetComponent<WeaponComponent>().WeaponType.IsEnumFlagPresent(a.WeaponType) &&
                !pawnInfo.Abilities.Contains(a.Id));

            if (ability == null)
                continue;

            options.Add(new AbilityPrizeItem(pawnInfo, ability));
        }

        Options = options;
    }
}

public class BuffPrize : BasePrize<BuffPrizeItem>
{
    public BuffPrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomBuff =
            contentManager.AvailableBuffs
                .OrderBy(_ => Guid.NewGuid())
                .ToList();

        var pawns = selectedParty.OrderBy(_ => Guid.NewGuid()).Take(numberOfOptions).ToList();

        var options = new List<BuffPrizeItem>();

        foreach (var pawnInfo in pawns)
        {
            var buff = randomBuff.FirstOrDefault(a => !pawnInfo.Buffs.Contains(a.Id));

            if (buff == null)
                continue;

            options.Add(new BuffPrizeItem(pawnInfo, buff));
        }

        Options = options;
    }
}

public class ConsumablePrize : BasePrize<ConsumablePrizeItem>
{
    public ConsumablePrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomConsumable = contentManager.AvailableConsumables
            .OrderBy(_ => Guid.NewGuid())
            .Take(numberOfOptions)
            .ToList();

        var pawns = selectedParty.OrderBy(_ => Guid.NewGuid()).Take(numberOfOptions).ToList();

        var options = new List<ConsumablePrizeItem>();

        foreach (var pawnInfo in pawns)
        {
            var consumable = randomConsumable.FirstOrDefault();

            if (consumable == null)
                continue;

            options.Add(new ConsumablePrizeItem(pawnInfo, consumable));
        }

        Options = options;
    }
}