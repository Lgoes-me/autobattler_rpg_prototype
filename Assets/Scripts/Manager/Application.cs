using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Application : MonoBehaviour
{
    public static Application Instance { get; private set; }
    private Dictionary<Type,IManager> Managers { get; set; }
    
    [field: SerializeField] public CinemachineBrain MainCamera { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        Managers = new Dictionary<Type, IManager>();
        
        Register(GetComponentInChildren<SceneManager>());
        Register(GetComponentInChildren<PlayerManager>());
        Register(GetComponentInChildren<PauseManager>());
        Register(GetComponentInChildren<PartyManager>());
        Register(GetComponentInChildren<AudioManager>());
        Register(GetComponentInChildren<InputManager>());
        Register(GetComponentInChildren<InterfaceManager>());
        Register(GetComponentInChildren<ContentManager>());
        Register(GetComponentInChildren<DialogueManager>());
        Register(GetComponentInChildren<TimeManager>());
        Register(GetComponentInChildren<TextManager>());

        Register(new GameSaveManager());
        Register(new ConfigManager());
        Register(new BattleEventsManager());
        Register(new BlessingManager());
        Register(new TutorialManager());
        Register(new ArchetypeManager());
        Register(new PrizeManager());
        Register(new ConsumableManager());
    }
    
    private void Start()
    {
        var gameSaveManager = GetManager<GameSaveManager>();
        var configManager = GetManager<ConfigManager>();
        var sceneManager = GetManager<SceneManager>();
        var timeManager = GetManager<TimeManager>();
        
        gameSaveManager.Prepare();
        configManager.Prepare();
        GetManager<ConsumableManager>().Prepare();
        GetManager<BattleEventsManager>().Prepare();
        GetManager<BlessingManager>().Prepare();
        GetManager<InputManager>().Prepare();
        GetManager<PartyManager>().Prepare();
        GetManager<SceneManager>().Prepare();
        GetManager<TutorialManager>().Prepare();
        GetManager<ArchetypeManager>().Prepare();
        GetManager<PrizeManager>().Prepare();

        configManager.Init();

        if (gameSaveManager.FirstTimePlaying())
        {
            gameSaveManager.StartNewSave();
            timeManager.StartClock();
            
            sceneManager.StartGameIntro();
        }
        else
        {
            sceneManager.StartGameMenu();
        }
    }
    
    private void Register(IManager manager)
    {
        Managers.Add(manager.GetType(), manager);
    }
    
    public T GetManager<T>() where T : IManager
    {
        return (T)Managers[typeof(T)];
    }
}

public interface IManager
{
    
}