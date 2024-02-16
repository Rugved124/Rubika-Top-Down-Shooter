using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Nanny : Enemy
{
    public float runawayRadius = 5f;
    [SerializeField]
    List<Collider> enemiesTemp;
    [SerializeField]
    private LayerMask enemies;
    bool canInvoke;

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    float bulletSpeed;

    [SerializeField]
    Transform shootPoint;

    [SerializeField]
    float dashSpeed;
    bool invokeDash;
    bool canInvokeBullet;

    public GameObject nannyTrail;
    public override void Start()
    {
        base.Start();
        enemyType = EnemyType.NANNY;
        Debug.Log(enemyType.ToString());
        canInvoke = true;
        canInvokeBullet = true;
        invokeDash = true;
        canShield = true;
        canDash = true;
        canDashAgain = true;
    }

    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(NannyIdleState), new NannyIdleState(this)},
            { typeof(ShieldState), new ShieldState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)},
            { typeof(NannyRunToAllyState), new NannyRunToAllyState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(NannyAttackState), new NannyAttackState(this)},
            { typeof(NannyDashState), new NannyDashState(this)},
            { typeof(NannyTiredState), new NannyTiredState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);

    }

    public override void Update()
    {
        base.Update();
        lowHpEnemy.RemoveAll(s => s == null);
        lowHpEnemy.RemoveAll(s => s.isShielded == true);
        LookForAllies();
        if(!canDash)
        {
            if (invokeDash)
            {
                invokeDash = false;
                Invoke("ResetCompleteDash", 10f);
            }
            
        }
    }
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
        Quaternion lookOnLook = Quaternion.LookRotation(pc.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * angularSpeedMulitplier);
    }
    public override void LookForAllies()
    {
        enemiesTemp = Physics.OverlapSphere(transform.position, enemyData.allyDetectionRange, enemies).ToList();
        if (enemiesTemp.Count > 0)
        {
            foreach (Collider i in enemiesTemp)
            {
                var enemy = i.GetComponent<Enemy>();

                if (enemy.currentHP / enemy.maxHP * 100f <= 50)
                {
                    if (enemy != this && !lowHpEnemy.Exists(r => r.gameObject == enemy.gameObject) && !enemy.isShielded)
                    {
                            lowHpEnemy.Add(enemy);
                    }
                }               
            }
        }
        lowHpEnemy = lowHpEnemy.OrderBy(enemy => enemy.currentHP / enemy.maxHP * 100f).ToList();


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, enemyData.allyDetectionRange);
    }
  
    public override void FireWeapon()
    {
        base.FireWeapon();
        LookAtPlayer();
        if (!isWeaponFiringDone)
        {
            Debug.Log("FIRED");
            isWeaponFiringDone = true;
            Rigidbody rb = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.transform.forward = transform.forward;
        }
        if (isWeaponFiringDone)
        {
            if (canInvokeBullet)
            {
                canInvokeBullet = false;
                Invoke("ResetAttack",enemyData.timeBetweenBullets);
            }
        }
    }
    public override void ResetShield()
    {
        base.ResetShield();
        if (canInvoke)
        {
            Invoke("InvokeShield", 7f);
            canInvoke = false;
        }
        
    }

    private void InvokeShield()
    {
        canShield = true;
        canInvoke = true;
    }
    public override void ResetAttack()
    {
        base.ResetAttack();
        isWeaponFiringDone = false;
        canInvokeBullet = true;
    }

    public override void Dash()
    {
        agent.speed = agent.speed * 5;
        agent.acceleration = agent.acceleration * 5;
        transform.forward = Quaternion.AngleAxis(UnityEngine.Random.Range(-70f, 70f), Vector3.up) * transform.forward;
        dashPos = transform.position + transform.forward * 10;
        //rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);
    }

    public override void ResetDash()
    {
        
        Invoke("DashInvoke", 1f);
    }
    private void DashInvoke()
    {
        canDashAgain = true;
        rb.isKinematic = true;
        agent.speed = agent.speed / 5;
        agent.acceleration = agent.acceleration / 5;
        transform.forward = pc.transform.position - transform.position;
    }

    private void ResetCompleteDash()
    {
        canDash = true;
        invokeDash = true;
    }

    public override void ReleaseNannyFire()
    {
        Instantiate(nannyTrail, transform.position, Quaternion.identity);
    }
}
