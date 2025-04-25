using TMPro;
using UnityEngine;

public class ProfilePanel : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI PawnName { get; set; }
    [field: SerializeField] private Transform Content { get; set; }
    
    public void Select(Pawn pawn)
    {
        PawnName.SetText(pawn.Id);
        Content.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        Content.gameObject.SetActive(false);
    }
}
