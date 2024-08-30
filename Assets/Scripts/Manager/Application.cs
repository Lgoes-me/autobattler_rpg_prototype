using UnityEngine;

public class Application : MonoBehaviour
{
    public static Application Instance { get; private set; }
    
    [field: SerializeField] private SceneManager SceneManager { get; set; }
    [field: SerializeField] public PlayerManager PlayerManager { get; private set; }
    [field: SerializeField] public PauseManager PauseManager { get; private set; }
    
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
        SceneManager.StartGame();
    }
}