using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class RunAwayState : BaseState
{
	Enemy _enemy;
    private bool canSetRunAway;

    float timeToReturn;
    public RunAwayState(Enemy enemy) : base(enemy.gameObject)
	{
		_enemy = enemy;
	}
	public override void EnterState()
	{
        timeToReturn = _enemy.enemyData.runAwayFor;
        _enemy.enemyAnim.SetTrigger("RunState");
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        _enemy.ResetAttack();
        canSetRunAway = true;
    }

	public override Type ExecuteState()
	{
        timeToReturn -= Time.deltaTime;
        if (_enemy.isInGravity)
        {
            return typeof(SuckedState);
        }
        if(_enemy.currentShield != null)
        {
            _enemy.canRunAway = false;
            _enemy.SetRunAwayToTrue();
            if (_enemy.enemyType == Enemy.EnemyType.NANNY)
            {
                return typeof(NannyIdleState);
            }
            return typeof(IdleState);
        }
        if (timeToReturn <= 0)
        {
            _enemy.canRunAway = false;
            _enemy.SetRunAwayToTrue();
            if (_enemy.enemyType == Enemy.EnemyType.NANNY)
            {
                return typeof(NannyIdleState);
            }
            return typeof(IdleState);
        }
		if(_enemy.enemyType == Enemy.EnemyType.NANNY && _enemy.hpPercent > 20f)
		{

            Vector3 runDir = _enemy.transform.position - _enemy.pc.transform.position;
            _enemy.agent.SetDestination(transform.position + runDir);
            if (_enemy.lowHpEnemy.Count > 0 && _enemy.canShield)
            {
                return typeof(NannyRunToAllyState);
            }
            else if (Vector3.Distance(_enemy.transform.position, _enemy.pc.transform.position) > _enemy.enemyData.attackRange)
            {
                return typeof(NannyIdleState);
            }
        }
        else if (_enemy.hpPercent <= 20f)
        {
            _enemy.RunAwayWhenLow();
        }

		return null;
	}
}

