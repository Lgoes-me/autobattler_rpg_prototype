using System.Collections.Generic;
using System.Linq;

public class ArchetypeManager : IManager
{
    private InterfaceManager InterfaceManager { get; set; }
    
    public List<ArchetypeData> Archetypes { get; private set; }
    
    public void Prepare()
    {
        InterfaceManager = Application.Instance.GetManager<InterfaceManager>();
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
        
            //Archetypes.Add(ArchetypeFactory.CreateArchetype(pair.Key, pair.Count));
        }

        InterfaceManager.UpdateProfileCanvas(party);
        InterfaceManager.UpdateArchetypesCanvas(Archetypes);
    }
}