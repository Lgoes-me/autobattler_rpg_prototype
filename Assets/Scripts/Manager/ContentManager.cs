using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentManager : MonoBehaviour, IManager
{
    [field: SerializeField] private List<PawnData> AvailablePawns { get; set; }
    [field: SerializeField] public List<WeaponData> AvailableWeapons { get; private set; }

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
        return AvailableWeapons.First(w => w.Id == id);
    }

#if UNITY_EDITOR
    [ContextMenu("ForceLoadContent")]
    void DoSomething()
    {
        AvailablePawns = Extensions.FindAllScriptableObjectsOfType<PawnData>("Assets/Datas/Pawns/Player");
        AvailableWeapons = Extensions.FindAllScriptableObjectsOfType<WeaponData>();
    }
#endif
}