﻿using System;
using System.Collections.Generic;
using System.Linq;

public class BasePrize<T>
{
    //nome, imagem, explicação
    public Dictionary<string, T> Options { get; set; }

    public T ChooseIndexPrize(string id)
    {
        return Options[id];
    }
}

public class LevelUpPrize : BasePrize<PawnInfo>
{
    public LevelUpPrize(int numberOfOptions, List<PawnInfo> selectedParty)
    {
        var partyOrderedByLevel = 
            selectedParty.
                Where(p => p.CanLevelUp()).
                OrderByDescending(p => p.Level).
                ToList();
        
        Options = 
            partyOrderedByLevel.
                Where(p => p.Level == partyOrderedByLevel[0].Level).
                OrderBy(p => Guid.NewGuid()).
                Take(numberOfOptions).
                ToDictionary(p => p.Name, p => p);
    }
}

public class BlessingPrize : BasePrize<BlessingIdentifier>
{
    public BlessingPrize(int numberOfOptions, List<BlessingIdentifier> blessings)
    {
        Options = 
            blessings
            .OrderBy(b => Guid.NewGuid())
            .Take(numberOfOptions)
            .ToDictionary(b => b.ToString(), b => b);
    }
}

public class PartyMemberPrize : BasePrize<PawnInfo>
{
    public PartyMemberPrize(int numberOfOptions, int level, List<PawnController> pawns, ContentManager contentManager)
    {
        var ids = pawns.Select(p => p.Pawn.Id).ToList();
        
        Options = 
            contentManager.AvailablePawns
                .Where(p => !ids.Contains(p.Id))
                .OrderBy(b => Guid.NewGuid())
                .Take(numberOfOptions)
                .ToDictionary(
                    b => b.ToString(), 
                    p => new PawnInfo(p.Id, level, 0, PawnStatus.Transient, p.Weapon, p.Abilities));
    }
}

public class WeaponPrize : BasePrize<WeaponPrizeResponse>
{
    public WeaponPrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomWeapons =
            contentManager.AvailableWeapons
                .OrderBy(b => Guid.NewGuid())
                .ToList();
        
        var partyOrderedByWeaponLevel = 
            selectedParty.
                OrderByDescending(p =>
                {
                    if (string.IsNullOrEmpty(p.Weapon))
                        return 0;

                    return contentManager.GetWeaponFromId(p.Weapon).Level;
                }).
                ToList();
        
        var pawns = partyOrderedByWeaponLevel.
            Where(p => p.Level == partyOrderedByWeaponLevel[0].Level).
            OrderBy(p => Guid.NewGuid()).
            Take(numberOfOptions).
            ToList();

        var options = new Dictionary<string, WeaponPrizeResponse>();
        
        foreach (var pawnInfo in pawns)
        {
            var pawn = contentManager.GetBasePawnFromId(pawnInfo.Name);
            var weapon = randomWeapons.First(w => pawn.WeaponType.IsEnumFlagPresent(w.Type) && pawnInfo.Weapon != w.Id);

            options.Add($"{weapon.Id} to {pawnInfo.Name}", new WeaponPrizeResponse(pawnInfo, weapon));
        }
        
        Options = options;
    }

}

public class AbilityPrize : BasePrize<AbilityPrizeResponse>
{
    public AbilityPrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomAbilities =
            contentManager.AvailableAbilities
                .OrderBy(b => Guid.NewGuid())
                .ToList();
        
        var pawns = selectedParty.
            OrderBy(p => Guid.NewGuid()).
            Take(numberOfOptions).
            ToList();

        var options = new Dictionary<string, AbilityPrizeResponse>();
        
        foreach (var pawnInfo in pawns)
        {
            var pawn = contentManager.GetBasePawnFromId(pawnInfo.Name);
            var ability = randomAbilities.FirstOrDefault(a =>  pawn.WeaponType.IsEnumFlagPresent(a.WeaponType) && !pawnInfo.Abilities.Contains(a.Id));

            if(ability == null)
                continue;

            options.Add($"{ability.Id} to {pawnInfo.Name}", new AbilityPrizeResponse(pawnInfo, ability));
        }
        
        Options = options;
    }
}

public class BuffPrize : BasePrize<BuffPrizeResponse>
{
    public BuffPrize(
        int numberOfOptions,
        ContentManager contentManager,
        List<PawnInfo> selectedParty)
    {
        var randomBuff =
            contentManager.AvailableBuffs
                .OrderBy(b => Guid.NewGuid())
                .ToList();
        
        var pawns = selectedParty.
            OrderBy(p => Guid.NewGuid()).
            Take(numberOfOptions).
            ToList();

        var options = new Dictionary<string, BuffPrizeResponse>();
        
        foreach (var pawnInfo in pawns)
        {
            var buff = randomBuff.FirstOrDefault(a =>  !pawnInfo.Buffs.Contains(a.Id));

            if(buff == null)
                continue;

            options.Add($"{buff.Id} to {pawnInfo.Name}", new BuffPrizeResponse(pawnInfo, buff));
        }
        
        Options = options;
    }
}

public class ConsumablePrize : BasePrize<ConsumableData>
{
    public ConsumablePrize(
        int numberOfOptions,
        ContentManager contentManager)
    {
        Options = 
            contentManager.AvailableConsumables
                .OrderBy(b => Guid.NewGuid())
                .Take(numberOfOptions)
                .ToDictionary(b => b.ToString(), b => b);
    }
}
