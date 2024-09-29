using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private List<EnemyData> Enemies { get; set; }

    private bool Active { get; set; }
    private Coroutine Coroutine { get; set; }

    private void Awake()
    {
        foreach (var enemyData in Enemies)
        {
            enemyData.PreparePawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }

            TryActivateEnemyArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Active)
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
            }

            Coroutine = StartCoroutine(DeactivateEnemiesCoroutine(other.transform));
        }
    }

    private IEnumerator DeactivateEnemiesCoroutine(Transform other)
    {
        yield return new WaitUntil(() => Vector3.Distance(other.position, transform.position) > 30);
        DeactivateEnemies();
    }

    private void TryActivateEnemyArea()
    {
        if (Application.Instance.Save.DefeatedEnemies.Contains(Id))
            return;

        Active = true;

        foreach (var enemy in Enemies)
        {
            enemy.EnemyController.Activate(StartBattle);
        }
    }

    private void DeactivateEnemies()
    {
        Enemies.ForEach(e => e.EnemyController.Deactivate());
    }

    private void StartBattle()
    {
        foreach (var enemy in Enemies)
        {
            enemy.EnemyController.Prepare();
        }

        Application.Instance.SceneManager.StartBattleScene(
            Id,
            Enemies.Select(e => e.EnemyController.PawnController).ToList());
    }

    private void OnValidate()
    {
        if(Id != string.Empty)
            return;
        
        Id = Guid.NewGuid().ToString();
    }
}

[System.Serializable]
public class EnemyData
{
    [field: SerializeField] public EnemyController EnemyController { get; set; }
    
    [field: SerializeField] private PawnData PawnData { get; set; }
    
    public PawnController PawnController => EnemyController.PawnController;

    public void PreparePawn()
    {
        PawnController.SetCharacter(PawnData);
    }
}