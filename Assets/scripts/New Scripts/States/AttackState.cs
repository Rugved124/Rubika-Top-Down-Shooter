using System;
using System.Collections;
using UnityEngine;

public class AttackState : BaseState
{
    private Enemy _enemy;
    float startTime;
    
    public AttackState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        startTime = 2.5f;

        if (!_enemy.agent.isStopped)
        {
            _enemy.agent.isStopped = true;
            _enemy.agent.updateRotation = false;
        }

    }

    public override Type ExecuteState()
    {
        startTime -= Time.deltaTime;
        _enemy.LookAtPlayer();
        if (!_enemy.isWeaponFiringDone)
        {
            if (startTime <= 0f)
            {
                _enemy.FireWeapon();
            }
        }
        else
        {
            return typeof(RunToPCState);
        }
        
      
        return null;
    }

}
