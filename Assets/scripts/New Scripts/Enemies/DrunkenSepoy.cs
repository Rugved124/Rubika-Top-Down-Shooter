using System;
using System.Collections.Generic;
using UnityEngine;

public class DrunkenSepoy : Enemy
{
    public GameObject fireline;


    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(AttackState), new AttackState(this)},
            { typeof(DeadState), new DeadState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);
    }
    public  override void FireWeapon ()
    {
        base.FireWeapon();

        fireline.SetActive(true);
        firedTime-= Time.deltaTime;

        if (firedTime <= 0)
        {
            fireline.SetActive(false);
            isWeaponFiringDone = true;
        }
    }

    public override void ResetAttack()
    {
        base.ResetAttack();

        firedTime = enemyData.timeBetweenBullets;
    }
    public override void LookAtPlayer()
    {
        base.LookAtPlayer();
        Quaternion lookOnLook = Quaternion.LookRotation(pc.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * angularSpeedMulitplier);
    }
}

