using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Nanny : Enemy
{

	public float runawayRadius = 5f;
	private bool isRunningAway = false;
	public override void Start()
	{
		base.Start();
		enemyType = EnemyType.NANNY;
		Debug.Log(enemyType.ToString());

	}

	public override void InitializeStateMachine()
	{
		Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
		{
			{ typeof(IdleState), new IdleState(this)},
			{ typeof(ShieldState), new ShieldState(this)},
			{ typeof(DeadState), new DeadState(this)},
			{ typeof(RunAwayState), new RunAwayState(this)}
		};

		GetComponent<FiniteStateMachine>().SetStates(states);

	}

	public override void Update()
	{
		base.Update();
	}

	public void ShieldEnemies()
	{

	}

	public override void LookAtPlayer()
	{
		base.LookAtPlayer();
		Quaternion lookOnLook = Quaternion.LookRotation(pc.transform.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * angularSpeedMulitplier);
	}

	private void RunAwayFromPC()
	{

	}
}
