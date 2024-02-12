using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class DrunkenSepoy : Enemy
{
    public GameObject fireline;
    public float fireTime;

    public int fireDamage;
    bool canInvoke = true;

    bool canAdd;

    [SerializeField]
    protected float visionConeAngle = 90f;

    Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);
    public override void Start()
    {

        base.Start();
        enemyType = EnemyType.DRUNKENSEPOY;
        Debug.Log(enemyType.ToString());
        isWeaponFiringDone = false;
        startingAngle = Quaternion.AngleAxis(-visionConeAngle / 2, Vector3.up);
        firedTime = fireTime;
    }

    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(SepoyAttackState), new SepoyAttackState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);

      
    }
    public  override void FireWeapon ()
    {
        base.FireWeapon();
        if (!isWeaponFiringDone)
        {
            fireline.SetActive(true);
            firedTime -= Time.deltaTime;
            if (firedTime <= 0)
            {
                fireline.SetActive(false);
                isWeaponFiringDone = true;
                if (canAdd && pc.statusEffects.burnNumber > 0)
                {
                    pc.statusEffects.burnNumber--;
                    canAdd = false;
                }
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
                                if (!canAdd)
                                {
                                    pc.statusEffects.burnNumber++;
                                    canAdd = true;
                                }
                            }
                            return;
                        }
                    }
                    if (pc != null)
                    {
                        if (canAdd && pc.statusEffects.burnNumber > 0)
                        {
                            pc.statusEffects.burnNumber--;
                            canAdd = false;
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
            }
        }
    }

    public override void ResetAttack()
    {
        base.ResetAttack();
        isWeaponFiringDone = false;
        fireline.SetActive(false);
        firedTime = fireTime;
        if (canAdd && pc.statusEffects.burnNumber > 0)
        {
            pc.statusEffects.burnNumber--;
            canAdd = false;
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
        if (pc != null)
        {
            if (canAdd && pc.statusEffects.burnNumber > 0)
            {
                pc.statusEffects.burnNumber--;
                canAdd = false;
            }
        }
        base.Die();
    }
}

