using System;
using System.Collections.Generic;
using UnityEngine;

public class NannyDashState : BaseState
{
    private Enemy _enemy;
    [SerializeField]
    int dashCount;
    public NannyDashState(Enemy enemy): base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        dashCount = 3;
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = false;
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
            _enemy.agent.SetDestination(_enemy.dashPos);
        }
        else
        {
            if(_enemy.canDashAgain == true)
            {
                _enemy.canDash = false;
                return typeof(NannyTiredState);
            }
            
        }
        return null;
    }
}
