﻿using System;
using UnityEngine;

public class Application : MonoBehaviour
{
    public static Application Instance { get; private set; }
    
    public Save Save { get; set; }

    [field: SerializeField] public PawnData PlayerStartingCharacter { get; private set; }
    [field: SerializeField] public SaveManager SaveManager { get; private set; }
    [field: SerializeField] public SceneManager SceneManager { get; private set; }
    [field: SerializeField] public PlayerManager PlayerManager { get; private set; }
    [field: SerializeField] public PauseManager PauseManager { get; private set; }
    [field: SerializeField] public PartyManager PartyManager { get; private set; }
    [field: SerializeField] public AudioManager AudioManager { get; private set; }
    
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
        Save = SaveManager.LoadData<Save>("Save.json") ?? new Save(PlayerStartingCharacter);
        PlayerManager.Init();
        PartyManager.Init();
        SceneManager.StartGame();
    }
}