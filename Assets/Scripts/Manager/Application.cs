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
    [field: SerializeField] public BattleEventsManager BattleEventsManager { get; private set; }
    [field: SerializeField] private BlessingManager BlessingManager { get; set; }
    [field: SerializeField] public InterfaceManager InterfaceManager { get; private set; }
    [field: SerializeField] public GameSaveManager GameSaveManager { get; private set; }
    [field: SerializeField] public ContentManager ContentManager { get; private set; }
    [field: SerializeField] public ConfigManager ConfigManager { get; private set; }
    [field: SerializeField] public DialogueManager DialogueManager { get; private set; }
    [field: SerializeField] public TutorialManager TutorialManager { get; private set; }
    
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
        GameSaveManager.Init();
        PartyManager.Init();
        BlessingManager.Init();
        SceneManager.StartGame();
    }
}