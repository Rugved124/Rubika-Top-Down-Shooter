using System;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Enemy
{
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private Transform shootPoint;

    public GameObject bullet;
    public GameObject shadowAOE;

    bool allowInvoke = true;
    [SerializeField]
    LayerMask ignorPC;

    //-------------------------Secondary Weapon Variables-----------------------------------
    [SerializeField]
    LayerMask ignoreAlliesAndBullets;
    private bool isSecondaryFired;
    [SerializeField]
    private float secondaryFireSpeed;
    private bool canInvoke = true;
    public override void Start()
    {
        base.Start();
        enemyType = EnemyType.SHADOW;
        isWeaponFiringDone = true;
        Debug.Log(enemyType.ToString());

    }
    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(ShadowAttackState), new ShadowAttackState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);


    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        isWeaponFiringDone = true;
        RaycastHit hit;
        Physics.Raycast(pc.transform.position, Vector3.down, out hit, ignorPC);
        Vector3 spawnPoint = new Vector3(hit.point.x, hit.point.y + 0.04f, hit.point.z);
        Instantiate(shadowAOE, spawnPoint, Quaternion.identity);
    }

    public override void SecondaryWeaponFire()
    {
        base.SecondaryWeaponFire();
        RaycastHit hit;
        Physics.Raycast(transform.position, pc.transform.position - transform.position, out hit, enemyData.attackRange / 2, ignoreAlliesAndBullets);
        Debug.DrawRay(transform.position, pc.transform.position - transform.position, Color.red);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                
                if (!isSecondaryFired)
                {
                    Debug.Log("Fire");
                    isSecondaryFired = true;
                    Rigidbody bulletRb = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                    bulletRb.AddForce((pc.transform.position - shootPoint.position).normalized * bulletSpeed, ForceMode.Impulse);
                }
                if (isSecondaryFired)
                {
                    if (canInvoke)
                    {
                        canInvoke = false;
                        Invoke("ResetSecondary", secondaryFireSpeed);
                    }
                }
               
            }
        }
        
        
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

    private void ResetSecondary()
    {
        canInvoke = true;
        isSecondaryFired = false;

    }
}
