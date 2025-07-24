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
    [field: SerializeField] public List<BlessingData> AvailableBlessings { get; private set; } 
    [field: SerializeField] public List<ArchetypeData> AvailableArchetypes { get; private set; } 

    public Pawn GetPawnFromId(string id)
    {
        return AvailablePawns
            .First(p => p.Id == id)
            .ToDomain();
    }

    public Pawn GetPawnFromInfo(PawnInfo pawnInfo)
    {
        return AvailablePawns
            .First(p => p.Id == pawnInfo.Name)
            .ToDomain(pawnInfo.Status, TeamType.Player);
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
        return AvailableBuffs.First(b => b.Id == id);
    }
    
    public ConsumableData GetConsumableFromId(string id)
    {
        return AvailableConsumables.First(c => c.Id == id);
    }
    
    public BlessingData GetBlessingFromId(string id)
    {
        return AvailableBlessings.First(b => b.name == id);
    }
    
    public ArchetypeData GetArchetypeFromId(ArchetypeIdentifier id)
    {
        return AvailableArchetypes.First(b => b.Identifier == id);
    }
    
#if UNITY_EDITOR
    [ContextMenu("ForceLoadContent")]
    private void ForceLoadContent()
    {
        AvailablePawns = Extensions.FindAllScriptableObjectsOfType<PawnData>("Assets/Datas/Pawns/Player");
        AvailableWeapons = Extensions.FindAllScriptableObjectsOfType<WeaponData>();
        AvailableAbilities = Extensions.FindAllScriptableObjectsOfType<AbilityData>().Where(a => !string.IsNullOrWhiteSpace(a.Id)).ToList();
        AvailableBuffs = Extensions.FindAllScriptableObjectsOfType<BuffData>(); 
        AvailableConsumables = Extensions.FindAllScriptableObjectsOfType<ConsumableData>();
        AvailableBlessings = Extensions.FindAllScriptableObjectsOfType<BlessingData>();
        AvailableArchetypes = Extensions.FindAllScriptableObjectsOfType<ArchetypeData>();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}