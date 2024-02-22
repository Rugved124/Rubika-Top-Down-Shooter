using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Butcher : Enemy
{
   
    [SerializeField]
    private float bulletSpeed_setInInspector;
    [SerializeField]
    private GameObject debries;
    [SerializeField]
    private Transform shootPoint;
    public GameObject bullet;

    public int chargeDamage;
    public float bulletDirMultiplier;

    bool allowInvoke = true;
    [HideInInspector]
    public bool isChargeHit;
    [HideInInspector]
    public bool isChargeHitWall;

    //------------------------------Debries Falling--------------------------
    Vector3 spawnPoint;
    public Collider[] colliders;
    public LayerMask spawnLayerMask;
    [SerializeField]
    float gap;
    float timeBetweenDebries;
    public override void Start()
    {
        base.Start();
        enemyType = EnemyType.BUTCHER;
        Debug.Log(enemyType.ToString());
        canDash = true;
        timeBetweenDebries = 0.4f;
        isChargeHitWall = false;
    }
    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(ButcherAttackState), new ButcherAttackState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)},
            { typeof(ButcherChargeState), new ButcherChargeState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);

        
    }
    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(transform.position,pc.transform.position) >= 4f)
        {
            bulletDirMultiplier = 2f;
        }
        else
        {
            bulletDirMultiplier = 0.5f;
        }
    }
    public override void FireWeapon()
    {
        base.FireWeapon();
        isWeaponFiringDone = true;
        BaseBullet bulletShot = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<BaseBullet>();
        Vector3 movementDirection = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;
        movementDirection = Quaternion.AngleAxis(-45f, Vector3.up) * movementDirection;
        movementDirection = ((pc.transform.position + movementDirection * bulletDirMultiplier) - shootPoint.position).normalized;
        bulletShot.BulletMovement(movementDirection);
    }

    public override void ResetAttack()
    {
        base.ResetAttack();
        if (allowInvoke)
        {
            allowInvoke = false;
            Invoke("SetFiringToTrue", firedTime);
        }
        
    }
    public override void SetFiringToTrue()
    {
        base.SetFiringToTrue();
        allowInvoke = true;
    }
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
        Quaternion lookOnLook = Quaternion.LookRotation(pc.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * angularSpeedMulitplier);
    }
    private void OnDestroy()
    { 
        lowHpEnemy.Remove(this.GetComponent<Enemy>());
    }
    public override void ResetDash()
    {
        base.ResetDash();
        Invoke("InvokeResetDash", 10f);
    }

    private void InvokeResetDash()
    {
        canDash = true;
    }
    public override void Die()
    {
        base.Die();
        isCharging = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging)
        {
            if (collision.collider.tag == "Obstacle")
            {
                Debug.Log("I hit a Wall");
                canGabbarCharge = false;
                if (!isChargeHitWall)
                {
                    
                    agent.isStopped = true;
                    agent.SetDestination(transform.position);
                    SpawnDebries();
                    isChargeHitWall = false;
                    Invoke("ResetChargeHitWall", 10f);
                }
            }
            if (collision.collider.tag == "Player")
            {
                if (!isChargeHit)
                {

                    if (FindObjectOfType<PC>() != null)
                    {
                        isChargeHit = true;
                        Invoke("ResetChargeDamage", 10f);
                        FindObjectOfType<PC>().TakeDamage(chargeDamage);
                        FindObjectOfType<PC>().KnockBack(transform.position, 14);
                    }
                }
            }
        }
    }
    void ResetChargeDamage()
    {
        isChargeHit = false;
    }
    void ResetChargeHitWall()
    {
        isChargeHitWall = false;
        canGabbarCharge = true;
    }

    void SpawnDebries()
    {
        for (int i = 0;i < 3;i++)
        {
            timeBetweenDebries -= Time.deltaTime;
            if(timeBetweenDebries <= 0f)
            {
                timeBetweenDebries = 0.4f;
                SpawnSpheres();
                
            }
            
        }
    }
    void SpawnSpheres()
    {
        int safetyNet = 0;
        bool canSpawn = false;
        bool dontSpawn = false;
        while (!canSpawn)
        {
            float x = UnityEngine.Random.Range(2.5f, -2.5f);
            float z = UnityEngine.Random.Range(2.5f, -2.5f);

            spawnPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            canSpawn = PreventOverlapSpawn(spawnPoint);
            if (canSpawn)
            {
                break;
            }
            safetyNet++;
            if (safetyNet > 50)
            {
                dontSpawn = true;
                Debug.Log("Too Many Attempts");
                break;

            }
        }
        if (!dontSpawn)
        {
            Instantiate(debries, spawnPoint, Quaternion.identity);
        }


    }
    private bool PreventOverlapSpawn(Vector3 _spawnPoint)
    {
        colliders = Physics.OverlapSphere(transform.position, 5f, spawnLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x + gap;
            float hieght = colliders[i].bounds.extents.z + gap;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.z - hieght;
            float upperExtent = centerPoint.z + hieght;

            if (_spawnPoint.x >= leftExtent && _spawnPoint.x <= rightExtent)
            {
                if (_spawnPoint.z >= lowerExtent && _spawnPoint.z <= upperExtent)
                {
                    return false;
                }
            }
        }
        return true;
    }
}


