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

public class WeaponPrize : BasePrize<PawnInfo>
{
    public WeaponPrize(int numberOfOptions, List<PawnInfo> selectedParty)
    {
        var options = selectedParty
            .OrderBy(b => Guid.NewGuid())
            .Take(numberOfOptions)
            .ToDictionary(p => p.Name, p => p);

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
