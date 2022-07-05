using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneralStats
{
    public GameObject GameObjectBind { get; protected set; }

    public delegate void StatsHandler(GeneralStats stats);
    public float MaxHp { get; protected set; }
    protected float currentHP;
    public virtual float CurrentHp
    {
        get
        {
            return currentHP;
        }
        protected set
        {
            currentHP = value;
            if (currentHP > MaxHp)
                currentHP = MaxHp;
            CheckIfDead();
        }
    }

    public float Damage { get; protected set; }
    public float WalkSpeed { get; protected set; }
    public float RunSpeed { get; protected set; }

    public GeneralStats(GameObject gameObjectBind)
    {
        GameObjectBind = gameObjectBind;
        ResetStat();
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHp -= damage;
    }

    public virtual void Heal(float healAmount)
    {
        CurrentHp += healAmount;
    }

    public virtual void ResetStat()
    {
        CurrentHp = MaxHp;
    }

    public virtual bool CheckIfDead()
    {
        if (CurrentHp > 0)
        {
            return false;
        }
        return true;
    }
}
