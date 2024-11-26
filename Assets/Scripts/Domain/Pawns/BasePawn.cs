using System.Collections.Generic;
using System.Linq;

public class BasePawn
{
    public string Id { get; private set; }
    public CharacterController Character { get; private set; }
    public WeaponController Weapon { get; private set; }
    public List<CharacterInfo> CharacterInfos { get; private set; }
    
    public BasePawn(
        string id,
        CharacterController character,
        WeaponController weapon,
        List<CharacterInfo> characterInfos)
    {
        Id = id;
        Character = character;
        Weapon = weapon;
        CharacterInfos = characterInfos;
    }

    public CharacterInfo GetCharacterInfo(string identifier)
    {
        return CharacterInfos.FirstOrDefault(i => i.Identifier == identifier) ??
               CharacterInfos.First(i => i.Identifier == "default");
    }
}