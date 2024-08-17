public class PawnDomain
{
    public int Health { get; set; }
    public int Attack { get; private set; }
    public int AttackRange { get; private set; }
    public int Size { get; private set; }
    public int Initiative { get; private set; }

    public StateType State { get; set; }

    public PawnDomain(int health, int attack, int attackRange, int size, int initiative)
    {
        Health = health;
        Attack = attack;
        AttackRange = attackRange;
        Initiative = initiative;
        Size = size;
        State = StateType.Idle;
    }
}

public enum StateType
{
    Idle,
    Move,
    Attack,
    Dead
}

public enum TeamType
{
    Player,
    Enemies
}