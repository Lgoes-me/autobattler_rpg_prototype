using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] public PawnData PawnData { get; private set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PawnController PawnController { get; private set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }
   
    private List<string> Defeated { get; set; }

    private void Awake()
    {
        PawnController.SetCharacter(PawnData);
    }

    public List<string> GetDefeated()
    {
        if (Defeated == null)
        {
            Defeated = new List<string>(Application.Instance.Save.DefeatedEnemies);
        }

        return Defeated;
    }
    
    public void AddDefeated(string enemy)
    {
        Defeated.Add(enemy);
        Application.Instance.Save.DefeatedEnemies.Add(enemy);
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);
    }
    
    public void ClearDefeated()
    {
        Defeated?.Clear();
        Application.Instance.Save.DefeatedEnemies.Clear();
        Application.Instance.SaveManager.SaveData(Application.Instance.Save);
    }
    
    public void SpawnPlayerAt(Transform location)
    {
        NavMeshAgent.enabled = false;
        PlayerController.transform.position = location.position;
        PlayerController.transform.forward = location.forward;
        NavMeshAgent.enabled = true;
    }

    public void PlayerToBattle()
    {
        PlayerController.enabled = false;
        PlayerController.Prepare();

        PawnController.enabled = true;
        NavMeshAgent.enabled = true;
    }

    public void PlayerToWorld()
    {
        PawnController.Deactivate();
        PlayerController.gameObject.SetActive(true);
        PlayerController.enabled = true;
        PlayerController.Prepare();
        
        Application.Instance.PartyManager.SetPartyToFollow(false);
    }

    public void SetNewCameraPosition(Transform cameraTransform)
    {
        PlayerController.SetNewCameraPosition(cameraTransform);
    }
    
    public PawnController GetPawnController()
    {
        enabled = false;
        return PawnController;
    }
}