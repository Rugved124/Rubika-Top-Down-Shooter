using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrunkenSepoyRuggy : BaseEnemyRuggy
{
	public EnemyStates currentSepoyState;
	public bool needToGo;

	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
		currentSepoyState = currentEnemyStates;
	}

	public override void Update()
	{
		base.Update();

		switch (currentSepoyState)
		{
			case EnemyStates.WANDER:

				if (NeedsDestination())
				{
					GetDestination();
				}

				navMeshAgent.SetDestination(destination);

				break;
		}
	}
}
