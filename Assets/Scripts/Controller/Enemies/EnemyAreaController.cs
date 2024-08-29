using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private List<EnemyController> Enemies { get; set; }
    private PlayerController Player { get; set; }

    private bool Active { get; set; }
    private Coroutine Coroutine { get; set; }

    public void Init(PlayerController player)
    {
        Player = player;
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

            Coroutine = StartCoroutine(DeactivateEnemiesCoroutine());
        }
    }

    private IEnumerator DeactivateEnemiesCoroutine()
    {
        yield return new WaitUntil(() => Vector3.Distance(Player.transform.position, transform.position) > 30);
        DeactivateEnemies();
    }

    private void TryActivateEnemyArea()
    {
        if (Application.Instance.PlayerManager.Defeated.Contains(Id))
            return;

        Active = true;

        foreach (var enemy in Enemies)
        {
            enemy.Activate(Player, StartBattle);
        }
    }

    private void DeactivateEnemies()
    {
        Enemies.ForEach(e => e.Deactivate());
    }

    private void StartBattle()
    {
        foreach (var enemy in Enemies)
        {
            enemy.Prepare();
        }

        Application.Instance.SceneManager.StartBattleScene(Id, Enemies);
    }

    private void OnValidate()
    {
        Id = Guid.NewGuid().ToString();
    }
}