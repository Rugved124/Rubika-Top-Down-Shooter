using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ButcherChargeState : BaseState
{
    private Enemy _enemy;
    public float waitBeforeIdle;
    Vector3 lastKnownPlayerPos;
    float rageTime;
    bool canCharge;
    public ButcherChargeState(Enemy enemy): base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        waitBeforeIdle = 6f;
        rageTime = 2f;
        canCharge = true;
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        _enemy.agent.acceleration *= 4;
        _enemy.agent.speed *= 4;
    }   

    public override Type ExecuteState()
    {
        waitBeforeIdle -= Time.deltaTime;
        rageTime -= Time.deltaTime;
        if(rageTime <= 0f && canCharge)
        {
            canCharge = false;
            lastKnownPlayerPos = _enemy.pc.transform.position;
        }
        if(canCharge == false)
        {
            _enemy.agent.SetDestination(lastKnownPlayerPos);
        }
        if(waitBeforeIdle <= 0f)
        {
            _enemy.ResetDash();
            _enemy.agent.acceleration /= 4;
            _enemy.agent.speed /= 4 ;
            return typeof(IdleState);
        }
        return null;
    }
}
