using UnityEngine;

public class TextManager : MonoBehaviour, IManager
{
    [field: SerializeField] private StringsData StringsData { get; set; }
}