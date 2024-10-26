using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendItemController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI PawnName { get; set; }

    private PawnData PawnData { get; set; }

    public FriendItemController Init(PawnData pawnData)
    {
        PawnData = pawnData;
        PawnName.SetText(PawnData.Id);

        return this;
    }
}