using System;
using System.Collections.Generic;
using UnityEngine;

public class NannyDashState : BaseState
{
    private Enemy _enemy;
    int dashCount;
    public NannyDashState(Enemy enemy): base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        dashCount = 3;
    }

    public override Type ExecuteState()
    {
        if(dashCount > 0)
        {
            if (_enemy.canDashAgain)
            {
                _enemy.canDashAgain = false;
                _enemy.Dash();
                _enemy.ResetDash();
                dashCount--;
            }
            
        }
        else
        {
            _enemy.canDash = false;
            return typeof(NannyIdleState);
        }
        return null;
    }
}
