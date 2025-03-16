using UnityEngine;

public class Application : MonoBehaviour
{
    public static Application Instance { get; private set; }
    
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public SceneManager SceneManager { get; private set; }
    [field: SerializeField] public PlayerManager PlayerManager { get; private set; }
    [field: SerializeField] public PauseManager PauseManager { get; private set; }
    [field: SerializeField] public PartyManager PartyManager { get; private set; }
    [field: SerializeField] public AudioManager AudioManager { get; private set; }
    [field: SerializeField] public InputManager InputManager { get; private set; }
    [field: SerializeField] public InterfaceManager InterfaceManager { get; private set; }
    [field: SerializeField] public ContentManager ContentManager { get; private set; }
    [field: SerializeField] public DialogueManager DialogueManager { get; private set; }
    [field: SerializeField] public PrizeManager PrizeManager { get; private set; }
    [field: SerializeField] public TimeManager TimeManager { get; private set; }
    
    public SaveManager SaveManager { get; private set; }
    public GameSaveManager GameSaveManager { get; private set; }
    public ConfigManager ConfigManager { get; private set; }
    public BattleEventsManager BattleEventsManager { get; private set; }
    public BlessingManager BlessingManager { get; private set; }
    public TutorialManager TutorialManager { get; private set; }
    
    private void Awake()
    {  
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SaveManager = new SaveManager();
        GameSaveManager = new GameSaveManager();
        ConfigManager = new ConfigManager();
        BattleEventsManager = new BattleEventsManager();
        BlessingManager = new BlessingManager();
        TutorialManager = new TutorialManager();
        
        GameSaveManager.Prepare();
        ConfigManager.Prepare();
        BattleEventsManager.Prepare();
        BlessingManager.Prepare();
        InputManager.Prepare();
        PartyManager.Prepare();
        PlayerManager.Prepare();
        SceneManager.Prepare();
        TutorialManager.Prepare();

        ConfigManager.Init();

        if (GameSaveManager.FirstTimePlaying())
        {
            GameSaveManager.StartNewSave();
            
            PartyManager.GetAndSpawnAvailableParty();
            BlessingManager.GetBlessingsAndInitCanvas();
            TimeManager.StartClock();
            
            SceneManager.StartGameIntro();
        }
        else
        {
            SceneManager.StartGameMenu();
        }
    }
}