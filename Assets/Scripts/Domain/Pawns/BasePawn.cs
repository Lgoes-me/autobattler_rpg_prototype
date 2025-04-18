using System.Collections.Generic;
using System.Linq;

public class BasePawn
{
    public string Id { get; private set; }
    public CharacterController Character { get; private set; }
    private List<CharacterInfo> CharacterInfos { get; set; }
    public List<AbilityData> Abilities { get; private set; }
    public WeaponData Weapon { get; protected set; }
    public WeaponType WeaponType { get; private set; }

    public BasePawn(string id,
        CharacterController character,
        List<CharacterInfo> characterInfos,
        List<AbilityData> abilities,
        WeaponData weapon,
        WeaponType weaponType)
    {
        Id = id;
        Character = character;
        CharacterInfos = characterInfos;
        Abilities = abilities;
        WeaponType = weaponType;
        Weapon = weapon;
    }

    public CharacterInfo GetCharacterInfo(string identifier)
    {
        return CharacterInfos.FirstOrDefault(i => i.Identifier == identifier) ??
               CharacterInfos.First(i => i.Identifier == "default");
    }
}