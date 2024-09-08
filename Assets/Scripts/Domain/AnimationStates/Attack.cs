using UnityEngine;

[System.Serializable]
public class Attack
{
    [field: SerializeField] public string Animation { get; set; }
    [field: SerializeField] public int Damage { get; set; }
    [field: SerializeField] public int Range { get; set; }
    [field: SerializeField] public float Delay { get; set; }
    [field: SerializeField] public bool SpecialAttack { get; set; }
}