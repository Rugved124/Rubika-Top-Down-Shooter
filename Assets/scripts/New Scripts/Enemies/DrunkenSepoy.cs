using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class DrunkenSepoy : Enemy
{
    public GameObject fireline;
    

    public int fireDamage;
    bool canInvoke = true;
    [SerializeField]
    protected float visionConeAngle = 90f;

    [SerializeField]
    protected float lastTick;

    Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);
    public override void Start()
    {
        lastTick = -enemyData.timeBetweenBullets;
        base.Start();
        enemyType = EnemyType.DRUNKENSEPOY;
        Debug.Log(enemyType.ToString());
        isWeaponFiringDone = false;
        startingAngle = Quaternion.AngleAxis(-visionConeAngle / 2, Vector3.up);
        firedTime = fireTime;
        AIManager.instance.AddToList(this);
    }

    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(SepoyAttackState), new SepoyAttackState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)},
            { typeof(SuckedState), new SuckedState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);

      
    }
    public  override void FireWeapon ()
    {
        base.FireWeapon();
        if (!isWeaponFiringDone)
        {
            enemyAnim.SetTrigger("AttackState");
            fireline.SetActive(true);
            firedTime -= Time.deltaTime;
            if (firedTime <= 0)
            {
                enemyAnim.SetTrigger("IdleState");
                fireline.SetActive(false);
                isWeaponFiringDone = true;
            }
            if (firedTime > 0)
            {
                RaycastHit hit;

                Quaternion angle = transform.rotation * startingAngle;

                Vector3 direction = angle * Vector3.forward;

                Vector3 pos = transform.position;

                for (int i = 0; i < (visionConeAngle / 5) + 1; i++)
                {
                    if (Physics.Raycast(pos, direction, out hit, enemyData.aggroRadius - 0.4f))
                    {
                        if (hit.collider.tag == "Player")
                        {
                            Debug.DrawRay(pos, direction * hit.distance, Color.red);
                            if (pc != null)
                            {
                                if (Time.time - lastTick >= enemyData.timeBetweenBullets)
                                {
                                    lastTick = Time.time;
                                    pc.TakeDamage(fireDamage);
                                }
                            }
                            return;
                        }
                        if (hit.collider.CompareTag("Shield"))
                        {
                            hit.collider.gameObject.GetComponent<ShieldBehaviour>().SetPoisonedForTime();
                        }
                    }
                    direction = stepAngle * direction;
                }
            }
            
            
        }
        
        else if (isWeaponFiringDone)
        {
            if (canInvoke)
            {
                canInvoke = false;
                Invoke("ResetAttack", enemyData.timeBetweenBullets);
                sepoyLookAtPlayer = true;
            }
        }
    }

    public override void ResetAttack()
    {
        base.ResetAttack();
        isWeaponFiringDone = false;
        fireline.SetActive(false);
        if (UnityEngine.Random.Range(0f, 100f) <= 50)
        {
            firedTime = 0.5f;
            sepoyLookAtPlayer = true;
        }
        else
        {
            firedTime = fireTime;
            Invoke("DontLookAtPlayer", 0.5f);
        }
        canInvoke = true;  
    }
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
        Quaternion lookOnLook = Quaternion.LookRotation(pc.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * angularSpeedMulitplier);
    }

    public override void Die()
    {
        base.Die();
    }

    void DontLookAtPlayer()
    {
        sepoyLookAtPlayer = false;
    }
}

