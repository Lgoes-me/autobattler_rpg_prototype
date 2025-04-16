using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentManager : MonoBehaviour, IManager
{
    [field: SerializeField] private List<PawnData> AvailablePawns { get; set; }
    [field: SerializeField] private List<WeaponData> AvailableWeapons { get; set; }

    public BasePawn GetBasePawnFromId(string id)
    {
        return AvailablePawns.First(p => p.Id == id).ToBaseDomain();
    }
    
    public Pawn GetPawnDomainFromInfo(PawnInfo pawnInfo)
    {
        return AvailablePawns.First(p => p.Id == pawnInfo.Name).ToDomain(TeamType.Player, 1);
    }

    public WeaponData GetWeaponFromId(string id)
    {
        return AvailableWeapons.First(p => p.Id == id);
    }
}