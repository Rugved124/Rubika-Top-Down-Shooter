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
    private Transform shootPoint;
    public GameObject bullet;

    public float bulletDirMultiplier;

    bool allowInvoke = true;

    public override void Start()
    {
        base.Start();
        enemyType = EnemyType.BUTCHER;
        Debug.Log(enemyType.ToString());

    }
    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(ButcherAttackState), new ButcherAttackState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)}
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
        Rigidbody bulletRB = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 movementDirection = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;
        movementDirection = Quaternion.AngleAxis(-45f, Vector3.up) * movementDirection;
        bulletRB.AddForce(((pc.transform.position + movementDirection * bulletDirMultiplier) - shootPoint.position).normalized * bulletSpeed_setInInspector, ForceMode.Impulse);
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
   
}
