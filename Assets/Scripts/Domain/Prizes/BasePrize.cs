using System;
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
    public PartyMemberPrize(List<BasePawn> pawns)
    {
        throw new NotImplementedException();
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
                OrderByDescending(p => p.Weapon?.Level ?? 0).
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
            var weapon = randomWeapons.First(w => w.Type == pawn.WeaponType && pawnInfo.Weapon?.Id != w.Id);

            options.Add($"{weapon.Id} to {pawnInfo.Name}", new WeaponPrizeResponse(pawnInfo, weapon));
        }
        
        Options = options;
    }

}
/*public class ConsumablePrize : BasePrize
{

}

public class AbilityPrize : BasePrize
{
    
}

public class BuffPrize : BasePrize
{
    
}*/
