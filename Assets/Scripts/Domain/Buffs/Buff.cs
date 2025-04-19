﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : IEnumerable
{
    public string Id { get; private set; }
    public float Duration { get; private set; }
    public PawnController Focus { get; private set; }
    public List<BuffComponent> Buffs { get; private set; }

    public int Priority { get; private set; }
    public string CharacterInfoIdentifier { get; private set; }

    private float StartingTime { get; set; }
    private bool Stackable { get; set; }
    private int Stacks { get; set; }
    
    public Buff(string id, float duration, bool stackable = false)
    {
        Id = id;
        Duration = duration;
        Buffs = new List<BuffComponent>();
        StartingTime = Time.time;
        Stackable = stackable;
        Stacks = 1;
    }

    public void Init(PawnController focus)
    {
        Focus = focus;
        Focus.ReceiveBuff(this);
    }

    public bool Tick()
    {
        if (Duration < 0 || Time.time - StartingTime < Duration)
            return false;

        foreach (var buff in Buffs)
        {
            buff.OnTick(Focus);
        }
        
        return true;
    }
    
    public void TryReapplyBuff()
    {
        Duration = Time.time;

        if (Stackable)
        {
            Stacks++;

            foreach (var buff in Buffs)
            {
                buff.ApplyStacks();
            }
        }
    }

    public void Add(BuffComponent item)
    {
        Buffs.Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Buffs.GetEnumerator();
    }
}