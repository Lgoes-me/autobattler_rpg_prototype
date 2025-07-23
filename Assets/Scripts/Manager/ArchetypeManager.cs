using System.Collections.Generic;
using System.Linq;

public class ArchetypeManager : IManager
{
    private InterfaceManager InterfaceManager { get; set; }
    private ContentManager ContentManager { get; set; }
    
    public List<ArchetypeData> Archetypes { get; private set; }
    
    public void Prepare()
    {
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
        ContentManager = Application.Instance.GetManager<ContentManager>();
        Archetypes = new List<ArchetypeData>();
    }

    public void CreateArchetypes(List<PawnController> party)
    {
        Archetypes.Clear();

        var archetypes = party
            .Select(p => p.Pawn.GetComponent<ArchetypesComponent>().Archetypes)
            .SelectMany(a => a)
            .GroupBy(a => a)
            .Select(g => new {g.Key, Count = g.Count()})
            .ToList();

        foreach (var pair in archetypes)
        {
            var archetype = ContentManager.GetArchetypeFromId(pair.Key);
            archetype.Setup(pair.Count);
            
            Archetypes.Add(archetype);
        }

        InterfaceManager.UpdateProfileCanvas(party);
        InterfaceManager.UpdateArchetypesCanvas(Archetypes);
    }
}