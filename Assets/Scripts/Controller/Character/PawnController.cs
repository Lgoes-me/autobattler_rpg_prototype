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

        if (Pawn.HasComponent<WeaponComponent>())
        {
            var weapon = pawn.GetComponent<WeaponComponent>().Weapon;
            CharacterController.SetWeapon(weapon);
        }

        if (CanvasController != null)
        {
            CanvasController.Init(Pawn);
        }

        Pawn.GetComponent<StatsComponent>().LostLife += ReceiveAttack;
        Pawn.GetComponent<StatsComponent>().GainedLife += ReceiveHeal;
    }

    public void UpdatePawn(PawnInfo pawnInfo)
    {
        Pawn.SetPawnInfo(pawnInfo);
        CharacterController.SetWeapon(Pawn.GetComponent<WeaponComponent>().Weapon);
    }

    public void RemoveCanvasController()
    {
        Destroy(CanvasController.gameObject);
        CanvasController = null;
    }

    public void StartBattle(Battle battle)
    {
        Pawn.GetComponent<StatsComponent>().StartBattle();

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

        if (ability.IsSpecial)
        {
            Application.Instance.GetManager<BattleEventsManager>().DoSpecialAttackEvent(Battle, this, ability);
        }
        else
        {
            Application.Instance.GetManager<BattleEventsManager>().DoAttackEvent(Battle, this, ability);
        }

        if (Pawn.TryGetComponent<WeaponComponent>(out var component) && component.Weapon != null)
        {
            foreach (var weaponEffect in component.Weapon.WeaponEffects)
            {
                if (weaponEffect.Type is not EffectType.InstantAction)
                    continue;

                foreach (var effectData in weaponEffect.Effects)
                {
                    var effect = effectData.ToDomain(this);
                    effect.ChooseFocus(this, Battle);
                    ability.AddEffect(effect);
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

        if (Pawn.TryGetComponent<StatsComponent>(out var statsComponent) && statsComponent.IsAlive)
            statsComponent.TickAllBuffs();

        if (Ability == null || !PawnState.CanWalk)
            return;

        if (Pawn.Focus == null || !Pawn.Focus.PawnState.CanBeTargeted)
            return;

        if (!PawnState.CanTransition)
            return;

        CharacterController.SetSpeed(NavMeshAgent.velocity.magnitude / NavMeshAgent.speed);
        var direction = Ability.WalkingDestination - transform.position;

        if (!Ability.ShouldUse())
        {
            NavMeshAgent.SetDestination(Ability.WalkingDestination);
            CharacterController.SetDirection(direction);
            CharacterController.SetAnimationState(new IdleState());
        }
        else
        {
            Ability.Used = true;
            NavMeshAgent.SetDestination(transform.position);

            CharacterController.SetAnimationState(new AbilityState(Ability, DoAbility, GoBackToIdle));
        }
    }

    public void FinishBattle()
    {
        Pawn.GetComponent<StatsComponent>().FinishBattle();

        CharacterController.SetAnimationState(new IdleState());

        enabled = false;
        Battle = null;
    }

    public void Dance()
    {
        CharacterController.SetAnimationState(new DanceState());
        NavMeshAgent.ResetPath();
    }

    public void SpawnProjectile(
        ProjectileController projectilePrefab,
        AnimationCurve trajectory,
        List<AbilityEffect> effects,
        PawnController focusedPawn,
        bool overrideSprite)
    {
        var roomScene = FindObjectOfType<RoomController>();

        var projectile = Instantiate(
                projectilePrefab,
                CharacterController.SpawnPoint.position,
                Quaternion.identity,
                roomScene.transform)
            .Init(
                this,
                effects,
                focusedPawn.transform.position,
                trajectory,
                Pawn.GetComponent<FocusComponent>().RangedAttackError);

        if (overrideSprite)
        {
            projectile.OverrideSprite(CharacterController.GetProjectileSprite());
        }
    }

    private void ReceiveAttack()
    {
        CharacterController.DoHitStop();

        if (Pawn.GetComponent<StatsComponent>().IsAlive)
            return;

        CharacterController.SetAnimationState(new DeadState());
        Ability = null;

        Application.Instance.GetManager<BattleEventsManager>().DoPawnDeathEvent(Battle, this);
    }

    private void ReceiveHeal()
    {
        CharacterController.DoNiceHitStop();

        if (!Pawn.GetComponent<StatsComponent>().IsAlive || PawnState is not DeadState)
            return;

        CharacterController.SetAnimationState(new IdleState());
    }

    public void SummonPawn(Pawn pawn)
    {
        var roomScene = FindObjectOfType<RoomController>();
        var prefab = Application.Instance.GetManager<PartyManager>().BasePawnPrefab;

        var randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)) * Vector3.forward * 1f;
        var pawnController = Instantiate(prefab, transform.position + randomRotation, Quaternion.identity,
            roomScene.transform);

        pawnController.Init(pawn);

        switch (pawn.Team)
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

        Pawn.GetComponent<StatsComponent>().LostLife -= ReceiveAttack;
        Pawn.GetComponent<StatsComponent>().GainedLife -= ReceiveHeal;
    }
}