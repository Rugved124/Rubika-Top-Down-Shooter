using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenShipoy : BaseEnemy
{
    public EnemyStates currentShipoyState;
    Vector3 playerLocation;
    [SerializeField]
    private float fireBreathingRange;

    Material myMaterial;
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        currentShipoyState = currentEnemyStates;
        myMaterial = GetComponent<Renderer>().material;
    }

    public override void Update()
    {
        base.Update();


        if (currentShipoyState != EnemyStates.ABILITY)
        {
            if (isPCDetected)
            {
                ChangeState(EnemyStates.CHASING);
            }
            else
            {
                ChangeState(EnemyStates.WANDER);
            }
        }
       

        switch(currentShipoyState)
        {
            case EnemyStates.WANDER:
                if (NeedsDestination())
                {
                    GetDestination();
                }
                navMeshAgent.SetDestination(destination);
                break;
            case EnemyStates.CHASING:
                if (CheckForPlayer() != null)
                {
                    playerLocation = CheckForPlayer().position;
                }
                navMeshAgent.SetDestination(playerLocation);
                if(navMeshAgent.remainingDistance <= fireBreathingRange)
                {
                    ChangeState(EnemyStates.ABILITY);
                }
                break;

            case EnemyStates.ABILITY:
                Debug.Log("Ability");
                ShipoyAttack();
                navMeshAgent.isStopped = true;
                break;
        }
    }
    public override void ChangeState(EnemyStates state)
    {
        
        if (currentShipoyState != state)
        {
            currentShipoyState = state;
        }
    }

    private void ShipoyAttack()
    {
        myMaterial.color = Color.red;
    }
}
