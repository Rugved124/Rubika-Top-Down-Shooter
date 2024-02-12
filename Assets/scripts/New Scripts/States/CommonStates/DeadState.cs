using System;
using System.Collections.Generic;


public class DeadState : BaseState
{
    private Enemy _enemy;

    public DeadState(Enemy enemy) : base(enemy.gameObject)
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
