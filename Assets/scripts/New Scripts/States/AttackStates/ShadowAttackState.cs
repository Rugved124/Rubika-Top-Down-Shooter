using System;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAttackState : BaseState
{
    private Enemy _enemy;
    private float waitBeforeTime;

    public ShadowAttackState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }
    public override void EnterState()
    {
        _enemy.isWeaponFiringDone = false;
        _enemy.agent.isStopped = true;
        _enemy.agent.updateRotation = false;
        waitBeforeTime = 5f;
        Debug.Log("I am in Attack State");
    }

    public override Type ExecuteState()
    {
        waitBeforeTime -= Time.deltaTime;
        float distanceFromPC = CalculateDistance(_enemy.pc.transform);
        if (_enemy.hpPercent > 20 && distanceFromPC <= _enemy.enemyData.attackRange/2)
        {

            _enemy.LookAtPlayer();
                if (!_enemy.isWeaponFiringDone)
                {
                    _enemy.FireWeapon();
                }
                if (_enemy.isWeaponFiringDone)
                {
                    _enemy.ResetAttack();
                }
        }
        if(waitBeforeTime <= 0f)
        {
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
