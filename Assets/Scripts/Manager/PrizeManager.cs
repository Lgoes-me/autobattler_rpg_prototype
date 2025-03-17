using UnityEngine;

public class PrizeManager : MonoBehaviour
{
    [field: SerializeField] private GameSaveManager GameSaveManager { get; set; }

    public async void CreateLevelUpPrize()
    {
        var prizes = new LevelUpPrize(3, GameSaveManager.GetSelectedParty());
    }

    public async void CreateBlessingPrize()
    {
        var prizes = new BlessingPrize(3);
    }
}
