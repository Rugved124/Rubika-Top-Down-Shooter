using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NannyIdleState : BaseState
{
    Enemy _enemy;
    bool roleDice;
    float chance;
    public NannyIdleState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;

    }
    public override void EnterState()
    {
        _enemy.agent.isStopped = true;
        _enemy.agent.updateRotation = false;
        roleDice = true;
    }

    public override Type ExecuteState()
    {

        float distanceFromPC = CalculateDistance(_enemy.pc.transform);
        _enemy.LookAtPlayer();
        if (_enemy.hpPercent <= 20 || distanceFromPC <= _enemy.enemyData.attackRange / 2f)
        {
            if(_enemy.canDash && _enemy.canRunAway)
            {
                if (roleDice)
                {
                    roleDice = false;
                    chance = UnityEngine.Random.Range(0f, 1f);
                }
                if(chance <= 0.3)
                {
                    return typeof(RunAwayState);
                }
                else
                {
                    return typeof(NannyDashState);
                }
            }
            if (_enemy.canRunAway)
            {
                return typeof(RunAwayState);
            }
            if (_enemy.canDash)
            {
                return typeof(NannyDashState);
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



