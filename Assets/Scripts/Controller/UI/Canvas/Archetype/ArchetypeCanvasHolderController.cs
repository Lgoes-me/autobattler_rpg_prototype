using System.Collections.Generic;
using UnityEngine;

public class ArchetypeCanvasHolderController : MonoBehaviour
{
    [field: SerializeField] private ArchetypeCanvasController ArchetypeCanvasControllerPrefab { get; set; }
    [field: SerializeField] private RectTransform ArchetypeCanvasParent { get; set; }
    
    private List<ArchetypeCanvasController> ArchetypeCanvases { get; set; }

    public void UpdateArchetypesCanvas(List<Archetype> archetypes)
    {
        ArchetypeCanvases ??= new List<ArchetypeCanvasController>();
        
        foreach (var archetypeCanvas in ArchetypeCanvases)
        {
            Destroy(archetypeCanvas.gameObject);
        }

        ArchetypeCanvases.Clear();

        foreach (var archetype in archetypes)
        {
            var archetypeCanvasController = Instantiate(ArchetypeCanvasControllerPrefab, ArchetypeCanvasParent).Init(archetype);
            ArchetypeCanvases.Add(archetypeCanvasController);
        }
    }

}