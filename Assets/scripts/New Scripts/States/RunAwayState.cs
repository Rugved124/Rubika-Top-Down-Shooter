using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunAwayState : BaseState
{
	Enemy _enemy;
	public RunAwayState(Enemy enemy) : base(enemy.gameObject)
	{
		_enemy = enemy;

	}
	public override void EnterState()
	{

	}

	public override Type ExecuteState()
	{

		float distanceFromPC = CalculateDistance(_enemy.pc.transform);
		if (distanceFromPC > _enemy.enemyData.attackRange) 
		{
			return typeof(ShieldState);
		}
		return null;
	}
	float CalculateDistance(Transform objTransform)
	{
		float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

		return distanceFromObj;
	}
}
