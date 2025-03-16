using System.Collections.Generic;

public class BasePrize
{
    public int NumberOfOptions { get; set; }
}

public class LevelUpPrize : BasePrize
{
    public List<PawnData> Pawns { get; set; }
}

public class BlessingPrize : BasePrize
{
    public List<BlessingIdentifier> Blessings { get; set; }
}

public class PartyMemberPrize : BasePrize
{
    public List<PawnData> Pawns { get; set; }
}

public class WeaponPrize : BasePrize
{
    
}

public class ConsumablePrize : BasePrize
{

}

public class AbilityPrize : BasePrize
{
    
}

public class BuffPrize : BasePrize
{
    
}
