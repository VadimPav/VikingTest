using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : GeneralStats
{
    public event StatsHandler monsterHPisZero;
    public event StatsHandler monsterDamageTaken;

    public float TimesDied { get; private set; }

    public void ReviveStats()
    {
        MaxHp = 1 + TimesDied;
        base.ResetStat();
    }

    public override void ResetStat()
    {
        TimesDied = 0;
        MaxHp = 1;
        Damage = 1;
        WalkSpeed = 3f;
        RunSpeed = 5;
        base.ResetStat();
    }

    public override void TakeDamage(float damage)
    {
        if (CurrentHp <= 0)
            return;
        base.TakeDamage(damage);
        monsterDamageTaken?.Invoke(this);
    }

    public override bool CheckIfDead()
    {
        if (!base.CheckIfDead())
        {
            return false;
        }
        monsterHPisZero?.Invoke(this);
        TimesDied++;
        return true;
    }

    public EnemyStats(GameObject gameObjectBind) : base(gameObjectBind)
    {
    }
}
