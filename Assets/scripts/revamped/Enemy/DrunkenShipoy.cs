using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenShipoy : BaseEnemy
{
    public EnemyStates currentShipoyState;
    Vector3 playerLocation;
    [SerializeField]
    private float fireBreathingRange;
    [SerializeField]
    private float fireBreathingChargeTime;

    private bool isCharged;

    Material myMaterial;

    [SerializeField]
    private float angularSpeedMulitplier;

    [SerializeField]
    private GameObject fireLine;
    bool isfiredOnce = false;
    float firedTime;
    float reFireTime = -3f;
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

        if (currentShipoyState != EnemyStates.CHARGING)
        {
            if (isPCDetected && Vector3.Distance(GetPlayerTransform().position, transform.position) > fireBreathingRange)
            {
                ChangeState(EnemyStates.CHASING);
            }
            else if (isPCDetected && Vector3.Distance(GetPlayerTransform().position, transform.position) <= fireBreathingRange && !isCharged)
            {
                ChangeState(EnemyStates.CHARGING);
            }
            else if (isPCDetected &&  isCharged)
            {
                ChangeState(EnemyStates.ATTACKING);
            }
            else
            {
                ChangeState(EnemyStates.WANDER);
            }
        }

        if (GetPlayerTransform() != null)
        {
            playerLocation = GetPlayerTransform().position;
        }


        switch (currentShipoyState)
        {
            case EnemyStates.WANDER:
                if (NeedsDestination())
                {
                    GetDestination();
                }
                navMeshAgent.SetDestination(destination);
                break;
            case EnemyStates.CHASING:
              
                navMeshAgent.SetDestination(playerLocation);
                
                break;

            case EnemyStates.CHARGING:

                //navMeshAgent.stoppingDistance = navMeshAgent.remainingDistance + 4;
                //navMeshAgent.SetDestination(playerLocation);
                navMeshAgent.isStopped = true;
                LookAtPlayer();

                if (!isCharged)
                {
                    ShipoyCharging();
                }
                break;

            case EnemyStates.ATTACKING:

                LookAtPlayer();
                ShipoyFire();
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

    private void ShipoyCharging()
    {
        myMaterial.color = Color.yellow;
        StartCoroutine(CoroutineToAttack());
        
    }

    IEnumerator CoroutineToAttack()
    {
        yield return new WaitForSeconds(fireBreathingChargeTime);
        myMaterial.color = Color.red;
        ChangeState(EnemyStates.ATTACKING);
        isCharged = true;
    }

    private void ShipoyFire()
    {
        
        if (!isfiredOnce && Time.time - reFireTime >= 3f)
        {
            firedTime = Time.time;
            fireLine.SetActive(true);
            isfiredOnce = true;
            Debug.Log(isfiredOnce);

        }
        if (isfiredOnce && Time.time - firedTime >= 5f)
        {
            fireLine.SetActive(false);
            if (isfiredOnce)
            {
                reFireTime = Time.time;
            }
            isfiredOnce = false;
            Debug.Log(isfiredOnce);
           
        }
    }

    private void LookAtPlayer()
    {
        Quaternion lookOnLook = Quaternion.LookRotation(playerLocation - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * angularSpeedMulitplier);
    }
}
