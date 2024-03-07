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
        startTime = 0.5f;
        if (!_enemy.agent.isStopped)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.updateRotation = false;
        }

    }

    public override Type ExecuteState()
    {
        if (_enemy.isInGravity)
        {
            return typeof(SuckedState);
        }
        startTime -= Time.deltaTime;
        if ((_enemy.hpPercent > 20  || _enemy.isShielded )&& Vector3.Distance(_enemy.pc.transform.position, transform.position) <= _enemy.enemyData.attackRange)
        {
            _enemy.LookAtPlayer();
            if (!_enemy.isWeaponFiringDone && startTime <= 0f)
            {
                _enemy.enemyAnim.SetTrigger("AttackState");
                _enemy.FireWeapon();
                //if(UnityEngine.Random.Range(0f, 100f) <= 50f && _enemy.canDash)
                //{
                //    _enemy.canDash = false;
                //   // return typeof(ButcherChargeState);
                //}
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
