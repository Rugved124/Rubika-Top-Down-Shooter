using System;
using System.Collections.Generic;
using UnityEngine;

public class ButcherAttackState : BaseState
{
    private Enemy _enemy;
    float startTime;

    public ButcherAttackState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {

        if (!_enemy.agent.isStopped)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.updateRotation = false;
        }

    }

    public override Type ExecuteState()
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
        if (Vector3.Distance(_enemy.pc.transform.position, transform.position) >= _enemy.enemyData.attackRange + 4f)
        {
            return typeof(ButcherRunToPCState);
        }

        return null;
    }

}
