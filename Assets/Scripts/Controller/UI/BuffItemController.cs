using System;
using TMPro;
using UnityEngine;

public class BuffItemController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI BuffName { get; set; }

    public BuffItemController Init(Buff buff)
    {
        BuffName.SetText(buff.Id);
        return this;
    }
}
