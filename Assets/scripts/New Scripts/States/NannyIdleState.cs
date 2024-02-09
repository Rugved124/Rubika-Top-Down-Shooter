using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NannyIdleState : BaseState
{
    Enemy _enemy;


    public NannyIdleState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;

    }
    public override void EnterState()
    {
        _enemy.agent.isStopped = true;
        _enemy.agent.updateRotation = false;
    }

    public override Type ExecuteState()
    {

        float distanceFromPC = CalculateDistance(_enemy.pc.transform);
        _enemy.LookAtPlayer();
        if(_enemy.lowHpEnemy.Count == 0) 
        {
            if (distanceFromPC <= _enemy.enemyData.attackRange)
            {
                return typeof(RunAwayState);
            }

        }
        else if(_enemy.lowHpEnemy.Count > 0 && _enemy.canShield)
        {
            return typeof(NannyRunToAllyState);
        }
       
        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}



