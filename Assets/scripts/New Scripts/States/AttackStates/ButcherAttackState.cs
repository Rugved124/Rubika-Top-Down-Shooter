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
        _enemy.canIdle = false;
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
        _enemy.LookAtPlayer();
        if ((_enemy.hpPercent > 20  || _enemy.isShielded || !_enemy.canRunAway)&& Vector3.Distance(_enemy.pc.transform.position, transform.position) <= _enemy.enemyData.attackRange)
        {
            if (!_enemy.isWeaponFiringDone && startTime <= 0f)
            {
                _enemy.canIdle = false;
                _enemy.isWeaponFiringDone = true;
                _enemy.enemyAnim.SetTrigger("AttackState");
                //_enemy.FireWeapon();
                if (_enemy.canDash)
                {
                    if (UnityEngine.Random.Range(0f, 100f) <= 80f)
                    {
                        _enemy.canDash = false;
                        return typeof(ButcherChargeState);
                    }
                }

            }
            if (_enemy.isWeaponFiringDone)
            {
                _enemy.ResetAttack();
            }
        }
        else if(_enemy.canIdle && _enemy.isWeaponFiringDone) 
        {
            return typeof(IdleState);
        }
        return null;
    }

}
