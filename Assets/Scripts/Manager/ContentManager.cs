using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContentManager : MonoBehaviour
{
    [field: SerializeField] public List<PawnData> AvailablePawns { get; private set; }

    public PawnFacade GetPawnFacadeFromId(string id)
    {
        return AvailablePawns.First(p => p.Id == id).ToFacade();
    }
    
    public Pawn GetPawnDomainFromFacade(PawnFacade facade)
    {
        return AvailablePawns.First(p => p.Id == facade.Id).ToDomain(TeamType.Player);
    }
}