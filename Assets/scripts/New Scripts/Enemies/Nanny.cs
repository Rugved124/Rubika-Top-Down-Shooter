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
    
    public override void Start()
    {
        base.Start();
        enemyType = EnemyType.NANNY;
        Debug.Log(enemyType.ToString());
        canInvoke = true;
        canShield = true;
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
        };

        GetComponent<FiniteStateMachine>().SetStates(states);

    }

    public override void Update()
    {
        base.Update();
        lowHpEnemy.RemoveAll(s => s == null);
        LookForAllies();
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
                    if (enemy != this && !lowHpEnemy.Exists(r => r.gameObject == enemy.gameObject))
                    {
                            lowHpEnemy.Add(enemy);
                    }
                }               
            }
        }
        lowHpEnemy = lowHpEnemy.OrderBy(enemy => enemy.currentHP).ToList();


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, enemyData.allyDetectionRange);
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
}

