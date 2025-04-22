using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PrizeOptionHolderController : MonoBehaviour
{
    [field: SerializeField] private GameObject PrizeCanvas { get; set; }
    [field: SerializeField] private PrizeOptionController PrizeOptionControllerPrefab { get; set; }
    
    public async Task<T> ShowPrizeCanvas<T>(BasePrize<T> prize) where T : BasePrizeItem
    {
        PrizeCanvas.SetActive(true);

        var tcs = new TaskCompletionSource<T>();

        var items = new List<PrizeOptionController>();
        
        foreach (var option in prize.Options)
        {
            items.Add(Instantiate(PrizeOptionControllerPrefab, PrizeCanvas.transform).Init(option, tcs));
        }

        var selectedPrize = await tcs.Task;

        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        
        items.Clear();
        
        PrizeCanvas.SetActive(false);
        
        return selectedPrize;
    }
}