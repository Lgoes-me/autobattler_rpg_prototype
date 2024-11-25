using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [field: SerializeField] private SaveManager SaveManager { get; set; }
    private Tutorial Tutorial { get; set; }

    public void Init()
    {
        Tutorial = SaveManager.LoadData<Tutorial>() ?? new Tutorial();
    }
}