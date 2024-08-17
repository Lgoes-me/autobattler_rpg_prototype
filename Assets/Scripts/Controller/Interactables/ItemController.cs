using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{
    public void Preselect()
    {
        Debug.Log($"Preselected {name}");
    }

    public void Select()
    {
        Debug.Log($"Selected {name}");
    }

    public void Unselect()
    {
        Debug.Log($"Unselected {name}");
    }
}