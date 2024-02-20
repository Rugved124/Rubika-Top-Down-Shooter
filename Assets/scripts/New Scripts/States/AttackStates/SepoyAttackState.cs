using System;
using System.Collections;
using UnityEngine;

public class SepoyAttackState : BaseState
{
    private Enemy _enemy;
    float startTime;
    public SepoyAttackState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        _enemy.sepoyLookAtPlayer = true;
        startTime = 1f;
        _enemy.isWeaponFiringDone = false;
        if (!_enemy.agent.isStopped)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.updateRotation = false;
        }
        if(UnityEngine.Random.Range(0f,100f) <= 50)
        {
            _enemy.firedTime = 0.5f;
            _enemy.sepoyLookAtPlayer = true;
        }
        else
        {
            _enemy.firedTime = _enemy.fireTime;
        }
    }

    public override Type ExecuteState()
    {
        if (_enemy.hpPercent > 20 && Vector3.Distance(transform.position, _enemy.pc.transform.position) <= _enemy.enemyData.attackRange)
        {
            startTime -= Time.deltaTime;
            if(startTime <= 0f)
            {
                if (_enemy.firedTime > 0.5)
                {
                    _enemy.sepoyLookAtPlayer = false;
                }
            }
            
            if (_enemy.sepoyLookAtPlayer)
            {
                _enemy.LookAtPlayer();
            }
            _enemy.FireWeapon();
        }
        else
        {
            return typeof(IdleState);
        }
        return null;
    }

}
