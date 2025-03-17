using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeOptionController : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI Name { get; set; }
    [field: SerializeField] private Button Button { get; set; }

    public PrizeOptionController Init(string optionKey, TaskCompletionSource<string> tcs)
    {
        Name.SetText(optionKey);
        Button.onClick.AddListener(() => tcs.SetResult(optionKey));

        return this;
    }
}