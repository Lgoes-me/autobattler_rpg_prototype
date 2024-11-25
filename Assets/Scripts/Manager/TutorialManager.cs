public class TutorialManager
{
    private Tutorial Tutorial { get; set; }
    private SaveManager SaveManager { get; set; }

    public void Prepare()
    {
        SaveManager = Application.Instance.SaveManager;
    }
    
    public void Init()
    {
        Tutorial = SaveManager.LoadData<Tutorial>() ?? new Tutorial();
    }
}