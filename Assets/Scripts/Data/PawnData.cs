using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] private int Health { get; set; }
    [field: SerializeField] private int Mana { get; set; }
    [field: SerializeField] public CharacterController Character { get; private set; }
    [field: SerializeField] public WeaponController Weapon { get; private set; }
    [field: SerializeField] private StatsData Stats { get; set; }
    [field: SerializeField] private List<AbilityData> Abilities { get; set; }
    [field: SerializeField] private List<AbilityData> SpecialAbilities { get; set; }

    public PawnDomain ToDomain()
    {
        var stats = Stats.ToDomain();
        return new PawnDomain(Id, Health, Mana, stats, Abilities, SpecialAbilities);
    }

    public PawnInfo ResetPawnInfo()
    {
        return new PawnInfo(Id, 0);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}