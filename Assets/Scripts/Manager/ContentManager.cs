using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentManager : MonoBehaviour, IManager
{
    [field: SerializeField] public List<PawnData> AvailablePawns { get; private set; }
    [field: SerializeField] public List<WeaponData> AvailableWeapons { get; private set; }
    [field: SerializeField] public List<AbilityData> AvailableAbilities { get; private set; }

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
    
    public AbilityData GetAbilityFromId(string id)
    {
        return AvailableAbilities.First(a => a.Id == id);
    }

#if UNITY_EDITOR
    [ContextMenu("ForceLoadContent")]
    void DoSomething()
    {
        AvailablePawns = Extensions.FindAllScriptableObjectsOfType<PawnData>("Assets/Datas/Pawns/Player");
        AvailableWeapons = Extensions.FindAllScriptableObjectsOfType<WeaponData>();
        AvailableAbilities = Extensions.FindAllScriptableObjectsOfType<AbilityData>().Where(a => !string.IsNullOrWhiteSpace(a.Id)).ToList();
    }
#endif
}