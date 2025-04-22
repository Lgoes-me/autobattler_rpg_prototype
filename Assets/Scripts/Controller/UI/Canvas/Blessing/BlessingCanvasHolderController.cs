using System;
using System.Collections.Generic;
using UnityEngine;

public class BlessingCanvasHolderController : MonoBehaviour
{
    [field: SerializeField] private List<BlessingCanvasController> BlessingCanvases { get; set; }

    private void Awake()
    {
        foreach (var blessingCanvas in BlessingCanvases)
        {
            blessingCanvas.Hide();
        }
    }
    
    public void UpdateBlessingsCanvas(List<Blessing> blessings)
    {
        foreach (var blessingCanvas in BlessingCanvases)
        {
            blessingCanvas.Hide();
        }
        
        for (var index = 0; index < BlessingCanvases.Count && index < blessings.Count; index++)
        {
            var blessingCanvas = BlessingCanvases[index];
            var blessing = blessings[index];

            blessingCanvas.Init(blessing);
        }
    }

}