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
    
    public Pawn GetPawnDomainFromInfo(PawnInfo pawnInfo)
    {
        return AvailablePawns.First(p => p.Id == pawnInfo.Name).ToDomain(TeamType.Player, 1);
    }
}