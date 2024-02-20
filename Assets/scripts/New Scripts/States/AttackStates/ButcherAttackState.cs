using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        if (_enemy.hpPercent > 20 && Vector3.Distance(_enemy.pc.transform.position, transform.position) <= _enemy.enemyData.attackRange)
        {
            _enemy.LookAtPlayer();
            if (!_enemy.isWeaponFiringDone)
            {
                _enemy.FireWeapon();
                if(UnityEngine.Random.Range(0f, 100f) <= 50f && _enemy.canDash)
                {
                    _enemy.canDash = false;
                    return typeof(ButcherChargeState);
                }
            }
            if (_enemy.isWeaponFiringDone)
            {
                _enemy.ResetAttack();
            }
        }
        else
        {
            return typeof(IdleState);
        }
        return null;
    }

}
