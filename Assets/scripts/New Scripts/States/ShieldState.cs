using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldState : BaseState
{
	Enemy _enemy;
	public ShieldState(Enemy enemy) : base(enemy.gameObject)
	{
		_enemy = enemy;

	}
	public override void EnterState()
	{

	}

	public override Type ExecuteState()
	{

		float distanceFromPC = CalculateDistance(_enemy.pc.transform);
		if (distanceFromPC <= _enemy.enemyData.attackRange) 
		{
			return typeof(RunAwayState);
		}
		
		return null;
	}
	float CalculateDistance(Transform objTransform)
	{
		float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

		return distanceFromObj;
	}
	//public void CheckForAllies()
	//{

	//	RaycastHit hit;

	//	Quaternion angle = _enemy.transform.rotation * startAngle;

	//	Vector3 direction = angle * Vector3.forward;

	//	Vector3 pos = _enemy.transform.position;

	//	for (int i = 0; i < (_enemy.enemyData.visionConeAngle / 5) + 1; i++)
	//	{

	//		if (Physics.Raycast(pos, direction, out hit, _enemy.enemyData.aggroRadius))
	//		{

	//			if (_enemy.pc != null)
	//			{
	//				Debug.DrawRay(pos, direction * hit.distance, Color.red);
	//				return;
	//			}
	//			else
	//			{
	//				Debug.DrawRay(pos, direction * hit.distance, Color.yellow);

	//			}
	//		}
	//		else
	//		{
	//			Debug.DrawRay(pos, direction * _enemy.enemyData.aggroRadius, Color.white);
	//		}


	//		direction = _enemy.enemyData.stepAngle * direction;
	//	}
	//}
}
