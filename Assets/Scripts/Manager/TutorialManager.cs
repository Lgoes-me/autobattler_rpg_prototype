public class TutorialManager : IManager
{
    private Tutorial Tutorial { get; set; }
    private SaveManager SaveManager { get; set; }

    public void Prepare()
    {
        SaveManager = new SaveManager();
    }
    
    public void Init()
    {
        Tutorial = SaveManager.LoadData<Tutorial>() ?? new Tutorial();
    }
}