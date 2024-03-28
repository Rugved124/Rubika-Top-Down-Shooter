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
        _enemy.isWeaponFiringDone = false;
        _enemy.agent.isStopped = true;
        _enemy.agent.updateRotation = false;
        Debug.Log("I am in Attack State");
    }

    public override Type ExecuteState()
    {
        float distanceFromPC = CalculateDistance(_enemy.pc.transform);
        if (distanceFromPC <= _enemy.enemyData.attackRange)
        { 
            _enemy.LookAtPlayer();
            if (!_enemy.isWeaponFiringDone)
            {
                _enemy.enemyAnim.SetTrigger("AttackState");
                _enemy.isWeaponFiringDone = true;
                _enemy.FireWeapon();
            }
        }
        else
        {
            if (!_enemy.isWeaponFiringDone || (_enemy.hpPercent < 20 && !_enemy.isShielded))
            {
                return typeof(IdleState);
            }
        }
        if(_enemy.storedAOE == null && _enemy.isWeaponFiringDone)
        {
            Debug.Log("Something");
            return typeof(ShadowTeleportState);
        }
        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}
