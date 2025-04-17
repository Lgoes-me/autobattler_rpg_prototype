[System.Serializable]
public class WeaponInfo
{
    public string Id { get; set; }
    public int Level { get; set; }

    public WeaponInfo()
    {
        Id = string.Empty;
        Level = 0;
    }
    
    public WeaponInfo(WeaponData weapon) : this()
    {
        Id = weapon.Id;
        Level = weapon.Level;
    }
}