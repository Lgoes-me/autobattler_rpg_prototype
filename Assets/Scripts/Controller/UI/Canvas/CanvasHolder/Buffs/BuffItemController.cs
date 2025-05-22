using TMPro;
using UnityEngine;

public class BuffItemController : BaseCanvasHolderItemController<Buff>
{
    [field: SerializeField] private TextMeshProUGUI BuffName { get; set; }

    public override BaseCanvasHolderItemController<Buff> Init(Buff buff)
    {
        BuffName.SetText(buff.Id);
        return this;
    }
}
