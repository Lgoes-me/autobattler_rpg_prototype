using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentManager : MonoBehaviour
{
    [field: SerializeField] private List<PawnData> AvailablePawns { get; set; }

    public BasePawn GetBasePawnFromId(string id)
    {
        return AvailablePawns.First(p => p.Id == id).ToBaseDomain();
    }
    
    public Pawn GetPawnDomainFromBase(BasePawn basePawn)
    {
        return AvailablePawns.First(p => p.Id == basePawn.Id).ToDomain(TeamType.Player, 1);
    }
}