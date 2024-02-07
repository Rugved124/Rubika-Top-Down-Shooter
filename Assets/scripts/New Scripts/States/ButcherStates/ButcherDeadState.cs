using System;
using System.Collections.Generic;
using UnityEngine;

public class ButcherDeadState : BaseState
{
    private Enemy _enemy;

    public ButcherDeadState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {

    }

    public override Type ExecuteState()
    {
        _enemy.Die();
        return null;
    }

}
