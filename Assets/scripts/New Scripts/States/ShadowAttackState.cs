using System;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAttackState : BaseState
{
    private Enemy _enemy;


    public ShadowAttackState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }
    public override void EnterState()
    {
        _enemy.isWeaponFiringDone = true;
        _enemy.agent.isStopped = true;
        _enemy.agent.updateRotation = false;
    }

    public override Type ExecuteState()
    {
        float distanceFromPC = CalculateDistance(_enemy.pc.transform);
        _enemy.LookAtPlayer();
        if (distanceFromPC >= _enemy.enemyData.attackRange / 2 && distanceFromPC <= _enemy.enemyData.attackRange)
        {
            
            if (!_enemy.isWeaponFiringDone)
            {
                _enemy.FireWeapon();
            }
            if (_enemy.isWeaponFiringDone)
            {
                _enemy.ResetAttack();
            }
        }
        else if (distanceFromPC < _enemy.enemyData.attackRange / 2)
        {
            _enemy.SecondaryWeaponFire();
        }
        if(distanceFromPC > _enemy.enemyData.attackRange)
        {
            return typeof(RunToPCState);
        }
        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}
