public class PawnFacade
{
    public string Id { get; private set; }
    public CharacterController Character { get;  private set; }
    public WeaponController Weapon { get;  private set; }
    
    public PawnFacade(
        string id,
        CharacterController character,
        WeaponController weapon)
    {
        Id = id;
        Character = character;
        Weapon = weapon;
    }
}