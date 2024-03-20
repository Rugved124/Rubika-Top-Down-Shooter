using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class ButcherChargeState : BaseState
{
    private Enemy _enemy;
    public float waitBeforeIdle;
    Vector3 dirTowardsPlayer;
    Vector3 lastKnownPlayerLoc;
    ObstacleAvoidanceType avoidanceType;
    float rageTime;
    bool canCharge;
    bool callSound;
    public ButcherChargeState(Enemy enemy): base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        callSound = true;
        waitBeforeIdle = 6f;
        rageTime = 2f;
        canCharge = true;
        _enemy.agent.enabled = false;
        _enemy.GetComponent<Rigidbody>().isKinematic = false;
        //_enemy.agent.isStopped = false;
        //_enemy.agent.radius = 0.1f;
        _enemy.canGabbarCharge = true;
        //_enemy.agent.acceleration *= 5f;
        //_enemy.agent.speed *= 5f;
        //_enemy.agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }   
        
    public override Type ExecuteState()
    { 
        waitBeforeIdle -= Time.deltaTime;
        rageTime -= Time.deltaTime;
        if(rageTime <= 1f && callSound)
        {
            callSound = false;
            _enemy.ability.Play();
        }
        if(rageTime <= 0f && canCharge)
        {
            canCharge = false;
            dirTowardsPlayer = (_enemy.pc.transform.position - _enemy.transform.position).normalized;
            lastKnownPlayerLoc = _enemy.transform.position + dirTowardsPlayer * 12;
        }
        if(canCharge == false)
        {
            _enemy.isCharging = true;
            if (_enemy.canGabbarCharge)
            {
                _enemy.dashVisual.SetActive(true);
                _enemy.currentVisual.SetActive(false);
                _enemy.transform.position += dirTowardsPlayer * _enemy.enemyData.chargeSpeed * Time.deltaTime;
                //_enemy.GetComponent<Rigidbody>().AddForce(dirTowardsPlayer * _enemy.enemyData.chargeSpeed * Time.deltaTime);
               // _enemy.agent.SetDestination(lastKnownPlayerLoc);
            }
        }
        if(waitBeforeIdle <= 0f || Vector3.Distance(_enemy.transform.position, lastKnownPlayerLoc) <= 0.5f || !_enemy.canGabbarCharge)
        {
            _enemy.dashVisual.SetActive(false);
            _enemy.currentVisual.SetActive(true);
            _enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _enemy.isCharging = false;
            _enemy.ResetDash();
            //_enemy.agent.acceleration /= 5f;
            //_enemy.agent.speed /= 5f;
            _enemy.GetComponent<Rigidbody>().isKinematic = true;
            _enemy.agent.enabled = true;
            return typeof(IdleState);
        }
        return null;
    }
}
