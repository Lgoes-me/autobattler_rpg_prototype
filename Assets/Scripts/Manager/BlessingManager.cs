﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlessingManager : MonoBehaviour
{
    [field: SerializeField] private InterfaceManager InterfaceManager { get; set; }
    [field: SerializeField] private GameSaveManager GameSaveManager { get; set; }

    public List<Blessing> Blessings { get; private set; }
    private BlessingFactory BlessingFactory { get; set; }

    public void Init()
    {
        BlessingFactory = new BlessingFactory();
        Blessings = GameSaveManager.GetBlessings().Select(j => BlessingFactory.CreateBlessing(j)).ToList();
        InterfaceManager.InitBlessingsCanvas(Blessings);
    }

    public void AddBlessing(BlessingIdentifier identifier)
    {
        Blessings.Add(BlessingFactory.CreateBlessing(identifier));
        GameSaveManager.SetBlessings();
    }

    public void RemoveBlessing(BlessingIdentifier identifier)
    {
        var jokerToRemove = Blessings.FirstOrDefault(j => j.Identifier == identifier);
        if (jokerToRemove == null)
            return;

        Blessings.Remove(jokerToRemove);
        GameSaveManager.SetBlessings();
    }

    public void ReorderBlessings(List<BlessingIdentifier> identifiers)
    {
        Blessings = identifiers.Select(j => BlessingFactory.CreateBlessing(j)).ToList();
        GameSaveManager.SetBlessings();
    }
}