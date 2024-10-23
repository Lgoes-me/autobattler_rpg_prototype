using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlessingManager : MonoBehaviour
{
    public List<BlessingIdentifier> BlessingsReadOnly => Blessings.Select(j => j.Identifier).ToList();
    public List<Blessing> Blessings { get; set; }
    private BlessingFactory BlessingFactory { get; set; }

    public void Init()
    {
        BlessingFactory = new BlessingFactory();
        Blessings = Application.Instance.Save.Blessings.Select(j => BlessingFactory.CreateBlessing(j)).ToList();
        
        Application.Instance.InterfaceManager.InitBlessingsCanvas(BlessingsReadOnly);
    }

    public void AddBlessing(BlessingIdentifier identifier)
    {
        Blessings.Add(BlessingFactory.CreateBlessing(identifier));
        SaveOperation();
    }

    public void RemoveBlessing(BlessingIdentifier identifier)
    {
        var jokerToRemove = Blessings.FirstOrDefault(j => j.Identifier == identifier);
        if (jokerToRemove == null)
            return;

        Blessings.Remove(jokerToRemove);
        SaveOperation();
    }

    public void ReorderBlessings(List<BlessingIdentifier> identifiers)
    {
        Blessings = identifiers.Select(j => BlessingFactory.CreateBlessing(j)).ToList();
        SaveOperation();
    }

    private void SaveOperation()
    {
        var save = Application.Instance.Save;
        save.Blessings = Blessings.Select(j => j.Identifier).ToList();
        Application.Instance.SaveManager.SaveData(save);
    }

}