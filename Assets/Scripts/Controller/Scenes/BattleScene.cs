using System.Collections.Generic;
using UnityEngine;

public class BattleScene : BaseScene
{
    [field: SerializeField] private ArenaController ArenaController { get; set; }
    [field: SerializeField] private PlayerArenaController PlayerArenaController { get; set; }
    
    public void ActivateBattleScene(List<EnemyController> enemies)
    {
        ArenaController.Init(enemies);
    }
}