using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private bool canInvoke;
    bool canTP = true;
    Transform tpPoint;
    Vector3 telePoint;
    float randomDistance;
    float randomTPAngle;
    public float teleportRange = 8.0f;
    public GameObject electricity;
    public override void Start()
    {
        base.Start();
        enemyType = EnemyType.SHADOW;
        isWeaponFiringDone = true;
        Debug.Log(enemyType.ToString());
        tpDone = false;
        enemyData.canTeleport = true;
        canInvoke = true;
        electricity.SetActive(false);
    }
    public override void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(IdleState), new IdleState(this)},
            { typeof(RunToPCState), new RunToPCState(this)},
            { typeof(ShadowAttackState), new ShadowAttackState(this)},
            { typeof(DeadState), new DeadState(this)},
            { typeof(RunAwayState), new RunAwayState(this)},
            { typeof(ShadowTeleportState), new ShadowTeleportState(this)},
            { typeof(SuckedState), new SuckedState(this)}
        };

        GetComponent<FiniteStateMachine>().SetStates(states);


    }

    public override void FireWeapon()
    {
        base.FireWeapon();
        RaycastHit hit;
        Physics.Raycast(pc.transform.position, Vector3.down, out hit, ignorPC);
        Vector3 spawnPoint = new Vector3(hit.point.x, hit.point.y + 0.04f, hit.point.z);
        storedAOE = Instantiate(shadowAOE, spawnPoint, Quaternion.identity);
        electricity.SetActive(true);
    }

    //public override void SecondaryWeaponFire()
    //{
    //    base.SecondaryWeaponFire();
    //    RaycastHit hit;
    //    Physics.Raycast(transform.position, pc.transform.position - transform.position, out hit, enemyData.attackRange / 2, ignoreAlliesAndBullets);
    //    Debug.DrawRay(transform.position, pc.transform.position - transform.position, Color.red);
    //    if(hit.collider != null)
    //    {
    //        if (hit.collider.tag == "Player")
    //        {

    //            if (!isSecondaryFired)
    //            {
    //                Debug.Log("Fire");
    //                isSecondaryFired = true;
    //                Rigidbody bulletRb = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
    //                bulletRb.AddForce((pc.transform.position - shootPoint.position).normalized * bulletSpeed, ForceMode.Impulse);
    //            }
    //            if (isSecondaryFired)
    //            {
    //                if (canInvoke)
    //                {
    //                    canInvoke = false;
    //                    Invoke("ResetSecondary", secondaryFireSpeed);
    //                }
    //            }

    //        }
    //    }
    //}
    public override void ResetAttack()
    {
        electricity.SetActive(false);
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

    //private void ResetSecondary()
    //{
    //    canInvoke = true;
    //    isSecondaryFired = false;
    //}

    public override void Teleport()
    {
        //Vector3 followPoint;
        base.Teleport();
        if (canTP)
        {
            //canTP = false;
            //randomDistance = UnityEngine.Random.Range(1.5f, 3f);
            //randomAngle = UnityEngine.Random.Range(-30f, 30f);
            if(RandomPoint(transform.position,teleportRange,out telePoint))
            {
                agent.Warp(telePoint);
                //tpPoint = Instantiate(bullet, pc.transform.position + followPoint, Quaternion.identity).GetComponent<Transform>();
            }
            //followPoint = CalculateTeleportPosition(randomAngle, randomDistance);
            //Invoke("InvokeTeleport", 5f);
            //enemyData.canTeleport = false;
        }
        //if (Vector3.Distance(tpPoint.position, pc.transform.position) > randomDistance)
        //{
        //    tpPoint.position += (pc.transform.position - tpPoint.position).normalized * 8 * Time.deltaTime;
        //}    
    }
    //private void InvokeTeleportCooldown()
    //{
    //    canInvoke = true;
    //    tpDone = false;
    //    canTP = true;
    //    enemyData.canTeleport = true;
    //}

    private void InvokeTeleport()
    {
        tpDone = true;
    }   
    //    if (canInvoke)
    //    {
    //        canInvoke = false;
    //        tpDone = true;
    //        Destroy(tpPoint.gameObject);
    //        Invoke("InvokeTeleportCooldown",0f);
    //    }
        
    //}

    public Vector3 CalculateTeleportPosition(float angle, float distance)
    {
        Vector3 teleportPosition;
        if(pc != null)
        {
            Vector3 temp = (pc.transform.position - transform.position);
            teleportPosition = (pc.transform.position + temp).normalized * distance;
            teleportPosition = Quaternion.AngleAxis(angle, Vector3.up) * teleportPosition;
            return teleportPosition;
        }
        return transform.position;
    }
   

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    public override void Die()
        {
            base.Die();
            Destroy(storedAOE);
        }
}
