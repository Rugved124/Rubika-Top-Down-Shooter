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
        startTime = 0.5f;
        _enemy.isWeaponFiringDone = false;
        if (!_enemy.agent.isStopped)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.updateRotation = false;
        }

    }

    public override Type ExecuteState()
    {
        if(Vector3.Distance(transform.position, _enemy.pc.transform.position) > _enemy.enemyData.attackRange)
        {
            return typeof(RunToPCState);
        }
        startTime -= Time.deltaTime;
        _enemy.LookAtPlayer();
        if (startTime <= 0f)
        { 
            _enemy.FireWeapon();
        }  
        return null;
    }

}
