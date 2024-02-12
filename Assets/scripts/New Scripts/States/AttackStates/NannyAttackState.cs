using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NannyAttackState : BaseState
{
    Enemy _enemy;
    public NannyAttackState(Enemy enemy): base( enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        Debug.Log("Attack");
    }

    public override Type ExecuteState()
    {
        float distanceFromPC = CalculateDistance(_enemy.pc.transform);

        if (_enemy.hpPercent > 20 && distanceFromPC > _enemy.enemyData.attackRange / 3 && distanceFromPC <= _enemy.enemyData.attackRange && (_enemy.lowHpEnemy.Count == 0 || !_enemy.canShield))
        {
            _enemy.FireWeapon();
        }
        else if (_enemy.isWeaponFiringDone)
        {
            return typeof(NannyIdleState);
        }
        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }

}
