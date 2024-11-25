using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Tutorial Tutorial { get; set; }
    private SaveManager SaveManager { get; set; }

    private void Start()
    {
        SaveManager = Application.Instance.SaveManager;
    }
    
    public void Init()
    {
        Tutorial = SaveManager.LoadData<Tutorial>() ?? new Tutorial();
    }
}