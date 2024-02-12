using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class RunAwayState : BaseState
{
	Enemy _enemy;
    public RunAwayState(Enemy enemy) : base(enemy.gameObject)
	{
		_enemy = enemy;

	}
	public override void EnterState()
	{
        Debug.Log("RunAway");
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        _enemy.ResetAttack();
    }

	public override Type ExecuteState()
	{
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

