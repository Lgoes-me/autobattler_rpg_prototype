using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PawnController : MonoBehaviour
{
    [field: SerializeField] private BasePawnCanvasController CanvasController { get; set; }
    [field: SerializeField] private NavMeshAgent NavMeshAgent { get; set; }

    private Coroutine BackToIdleCoroutine { get; set; }
    private Ability Ability { get; set; }

    public CharacterController CharacterController { get; private set; }
    public Pawn Pawn { get; private set; }
    public AnimationState PawnState => CharacterController.CurrentState;
    public Battle Battle { get; private set; }

    public void Init(Pawn pawn)
    {
        Pawn = pawn;

        CharacterController = Instantiate(pawn.GetComponent<CharacterComponent>().Character, transform);
        
        if (Pawn.TryGetComponent<MountComponent>(out var mountComponent) && mountComponent.Mount != null)
        {
            CharacterController.SetMount(mountComponent.Mount);
        }

        if (Pawn.TryGetComponent<LevelUpStatsComponent>(out var levelUpStatsComponent))
        {
            levelUpStatsComponent.ApplyLevel(levelUpStatsComponent.Level);
        }

        if (Pawn.TryGetComponent<WeaponComponent>(out var weaponComponent))
        {
            weaponComponent.ApplyWeaponType(Pawn.WeaponType);
            CharacterController.SetWeapon(weaponComponent);
        }

        if (CanvasController != null)
        {
            CanvasController.Init(Pawn);
        }

        Pawn.GetComponent<ResourceComponent>().LostLife += ReceiveAttack;
        Pawn.GetComponent<ResourceComponent>().GainedLife += ReceiveHeal;
        Pawn.GetComponent<ResourceComponent>().LostMana += LostMana;
        Pawn.GetComponent<ResourceComponent>().GainedMana += GainedMana;
    }

    public void UpdatePawn(PawnInfo pawnInfo)
    {
        Pawn.SetPawnInfo(pawnInfo);
        
        if (Pawn.TryGetComponent<WeaponComponent>(out var weaponComponent))
        {
            CharacterController.SetWeapon(weaponComponent);
        }
    }

    public void RemoveCanvasController()
    {
        Destroy(CanvasController.gameObject);
        CanvasController = null;
    }

    public void StartBattle(Battle battle)
    {
        Pawn.StartBattle();

        enabled = true;
        Ability = null;

        Battle = battle;
        CharacterController.SetAnimationState(new IdleState());
    }

    public void RealizeTurn()
    {
        if (PawnState is not IdleState)
            return;

        Ability = Pawn.GetComponent<AbilitiesComponent>().GetCurrentAttackIntent(this, Battle);

        if (Pawn.Focus == null || !Pawn.Focus.PawnState.CanBeTargeted)
        {
            Pawn.Focus = Battle.QueryEnemies(this, Pawn.GetComponent<FocusComponent>().EnemyFocusPreference);
        }

        Ability.ChooseFocus(this, Battle);
    }

    private void DoAbility(Ability ability)
    {
        if (!PawnState.AbleToFight)
            return;

        Application.Instance.GetManager<BattleEventsManager>().DoAttackEvent(this, ability);
        
        if (ability.IsSpecial)
        {
            Application.Instance.GetManager<BattleEventsManager>().DoSpecialAttackEvent(this, ability);
        }

        if (Pawn.TryGetComponent<WeaponComponent>(out var component) && component.Weapon != null)
        {
            foreach (var weaponEffect in component.Weapon.WeaponEffects)
            {
                foreach (var effectData in weaponEffect.Effects)
                {
                    var effect = effectData.ToDomain(this);
                    effect.ChooseFocus(this, Battle);
                    ability.AddEffect(effect, weaponEffect.Type);
                }
            }
        }

        Application.Instance.GetManager<AudioManager>().PlaySound(SfxType.Slash);

        ability.SpendResource();
        ability.DoAction();
    }

    private void GoBackToIdle()
    {
        if (BackToIdleCoroutine != null)
            StopCoroutine(BackToIdleCoroutine);

        NavMeshAgent.ResetPath();
        BackToIdleCoroutine = StartCoroutine(GoBackToIdleCoroutine());
    }

    private IEnumerator GoBackToIdleCoroutine()
    {
        yield return new WaitForSeconds(Ability.Delay);

        if (!PawnState.AbleToFight)
            yield break;

        Ability = null;

        CharacterController.SetAnimationState(new IdleState());

        RealizeTurn();
    }

    private void FixedUpdate()
    {
        if (Pawn == null)
            return;

        if (Pawn.TryGetComponent<ResourceComponent>(out var resourceComponent) && resourceComponent.IsAlive &&
            Pawn.TryGetComponent<PawnBuffsComponent>(out var pawnBuffsComponent))
            pawnBuffsComponent.TickAllBuffs();

        if (Ability == null || !PawnState.CanWalk)
            return;

        if (Pawn.Focus == null || !Pawn.Focus.PawnState.CanBeTargeted)
            return;

        if (!PawnState.CanTransition)
            return;

        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude / NavMeshAgent.speed);
        var lookDirection = Ability.FocusDestination - transform.position;
        
        if (!Ability.ShouldUse())
        {
            NavMeshAgent.SetDestination(Ability.WalkingDestination);
            CharacterController.SetDirection(lookDirection);
            CharacterController.SetAnimationState(new IdleState());
        }
        else
        {
            Ability.Used = true;
            NavMeshAgent.SetDestination(transform.position);
            CharacterController.SetDirection(lookDirection);
            CharacterController.SetSpeed(0);

            CharacterController.SetAnimationState(new AbilityState(Ability, DoAbility, GoBackToIdle));
        }
    }

    public void EndBattle()
    {
        Pawn.EndBattle();

        enabled = false;
        Battle = null;

        if (!Pawn.GetComponent<ResourceComponent>().IsAlive)
            return;
        
        CharacterController.SetAnimationState(new IdleState());
    }

    public void Dance()
    {
        NavMeshAgent.ResetPath();
        CharacterController.SetAnimationState(new DanceState());
    }
    
    public void Idle()
    {
        CharacterController.SetAnimationState(new IdleState());
    }

    public void SpawnProjectile(
        ProjectileController projectilePrefab,
        AnimationCurve trajectory,
        List<AbilityEffect> effects,
        PawnController focusedPawn,
        bool overrideSprite,
        List<AbilityBehaviour> extraEffects)
    {
        var roomScene = FindFirstObjectByType<RoomController>();

        var projectile = Instantiate(
                projectilePrefab,
                CharacterController.GetSpawnPoint(),
                Quaternion.identity,
                roomScene.transform)
            .Init(
                this,
                effects,
                focusedPawn.transform.position,
                trajectory,
                Pawn.GetComponent<FocusComponent>().RangedAttackError,
                extraEffects);

        if (overrideSprite)
        {
            projectile.OverrideSprite(CharacterController.GetProjectileSprite());
        }
    }
    
    private void ReceiveAttack(DamageDomain damageDomain)
    {
        CharacterController.DoHitStop();

        if (Pawn.GetComponent<ResourceComponent>().IsAlive)
            return;

        CharacterController.SetAnimationState(new DeadState());
        
        NavMeshAgent.ResetPath();
        NavMeshAgent.enabled = false;
        Ability = null;

        Application.Instance.GetManager<BattleEventsManager>().DoPawnDeathEvent(this, damageDomain);
        Application.Instance.GetManager<BattleEventsManager>().DoHealthLostEvent(this, damageDomain);
        
    }

    private void ReceiveHeal(int value)
    {
        Application.Instance.GetManager<BattleEventsManager>().DoHealthGainedEvent(this, value);
        
        CharacterController.DoNiceHitStop();

        if (!Pawn.GetComponent<ResourceComponent>().IsAlive || PawnState is not DeadState)
            return;
        
        NavMeshAgent.enabled = true;
        Ability = null;
        CharacterController.SetAnimationState(new IdleState());
        RealizeTurn();
    }

    private void LostMana(int value)
    {
        Application.Instance.GetManager<BattleEventsManager>().DoManaLostEvent(this, value);
    }
    
    private void GainedMana(int value)
    {
        Application.Instance.GetManager<BattleEventsManager>().DoManaGainedEvent(this, value);
    }
    
    public void SummonPawn(EnemyData enemyData)
    {
        var roomScene = FindFirstObjectByType<RoomController>();
        var prefab = Application.Instance.GetManager<PartyManager>().BasePawnPrefab;

        var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
        var pawnController = Instantiate(prefab, transform.position + randomRotation, Quaternion.identity,
            roomScene.transform);

        enemyData.PreparePawn(pawnController, Pawn.Team);

        switch (Pawn.Team)
        {
            case TeamType.Player:
                Battle.AddPlayerPawn(pawnController);
                break;
            case TeamType.Enemies:
                Battle.AddEnemy(pawnController);
                break;
        }

        pawnController.StartBattle(Battle);
        pawnController.RealizeTurn();
    }

    private void OnDestroy()
    {
        if (Pawn == null)
            return;

        Pawn.GetComponent<ResourceComponent>().LostLife -= ReceiveAttack;
        Pawn.GetComponent<ResourceComponent>().GainedLife -= ReceiveHeal;
    }
}