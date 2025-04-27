using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ContentManager : MonoBehaviour, IManager
{
    [field: SerializeField] public List<PawnData> AvailablePawns { get; private set; }
    [field: SerializeField] public List<WeaponData> AvailableWeapons { get; private set; }
    [field: SerializeField] public List<AbilityData> AvailableAbilities { get; private set; }
    [field: SerializeField] public List<BuffData> AvailableBuffs { get; private set; }
    [field: SerializeField] public List<ConsumableData> AvailableConsumables { get; private set; }

    public Pawn GetPawnFromId(string id)
    {
        return AvailablePawns
            .First(p => p.Id == id)
            .ToDomain();
    }

    public Pawn GetPawnDomainFromInfo(PawnInfo pawnInfo)
    {
        return AvailablePawns
            .First(p => p.Id == pawnInfo.Name)
            .ToDomain(pawnInfo.Status, TeamType.Player, 1);
    }

    public WeaponData GetWeaponFromId(string id)
    {
        return AvailableWeapons.First(w => w.Id == id);
    }
    
    public AbilityData GetAbilityFromId(string id)
    {
        return AvailableAbilities.First(a => a.Id == id);
    }
    
    public BuffData GetBuffFromId(string id)
    {
        return AvailableBuffs.First(a => a.Id == id);
    }
    
    public ConsumableData GetConsumableFromId(string id)
    {
        return AvailableConsumables.First(a => a.Id == id);
    }

#if UNITY_EDITOR
    [ContextMenu("ForceLoadContent")]
    void DoSomething()
    {
        AvailablePawns = Extensions.FindAllScriptableObjectsOfType<PawnData>("Assets/Datas/Pawns/Player");
        AvailableWeapons = Extensions.FindAllScriptableObjectsOfType<WeaponData>();
        AvailableAbilities = Extensions.FindAllScriptableObjectsOfType<AbilityData>().Where(a => !string.IsNullOrWhiteSpace(a.Id)).ToList();
        AvailableBuffs = Extensions.FindAllScriptableObjectsOfType<BuffData>(); 
        AvailableConsumables = Extensions.FindAllScriptableObjectsOfType<ConsumableData>();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}