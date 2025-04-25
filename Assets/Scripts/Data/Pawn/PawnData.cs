using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class PawnData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeReference] [field: SerializeField] private List<PawnDataComponent> Components { get; set; }

    public Pawn ToDomain()
    {
        var components = Components.Select(c => c.ToDomain()).ToList();
        return new Pawn(Id, components);
    }
    
    public Pawn ToDomain(TeamType team, int level)
    {
        var components = Components.Select(c => c.ToDomain()).ToList();
        return new Pawn(Id, components, team, level);
    }

    private void OnValidate()
    {
        if (Id != string.Empty)
            return;

        Id = name;
    }
}

[Serializable]
public class CharacterInfo
{
    [field: SerializeField] public string Identifier { get; set; }
    [field: SerializeField] public Sprite Portrait { get; set; }
    [field: SerializeField] public Sfx Audio { get; set; }
}
