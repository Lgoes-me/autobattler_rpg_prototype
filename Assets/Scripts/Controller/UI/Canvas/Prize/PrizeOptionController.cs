using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeOptionController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private Button Button { get; set; }

    public PrizeOptionController Init<T>(T option, TaskCompletionSource<T> tcs) where T : BasePrizeItem
    {
        Name.SetText(option.Name);
        Button.onClick.AddListener(() => tcs.SetResult(option));

        return this;
    }
}