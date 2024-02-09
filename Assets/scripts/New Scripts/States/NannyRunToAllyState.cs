using System;
using System.Collections.Generic;
using UnityEngine;

public class NannyRunToAllyState : BaseState
{
    Enemy _enemy;
    Enemy runAlly;
    public NannyRunToAllyState(Enemy enemy): base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        runAlly = _enemy.lowHpEnemy[0];
    }

    public override Type ExecuteState()
    {

        if (_enemy.lowHpEnemy.Count > 0)
        {
            if (_enemy.lowHpEnemy[0] != null)
            {
                _enemy.agent.SetDestination(_enemy.lowHpEnemy[0].transform.position);

                if (Vector3.Distance(_enemy.lowHpEnemy[0].transform.position, transform.position) <= _enemy.shieldDistance)
                {
                    return typeof(ShieldState);
                }
            }
            else
            {
                return typeof(NannyIdleState);
            }
            
        }
        
        return null;
    }
}
