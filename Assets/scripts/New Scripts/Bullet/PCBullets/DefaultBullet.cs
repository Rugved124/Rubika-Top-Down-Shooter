using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : BaseBullet
{
    public override void Start()
    {
        base.Start();

        this.bulletType = BulletTypes.DEFAULTAMMO;
        BulletMovement(pc.GetPCShoot().forward);
        //bulletRB = this.GetComponent<Rigidbody>();

    }
    public override void Update()
    {
        base.Update();
        

    }
    public override void InitializeBullet()
    {
        base.InitializeBullet();
    }
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }
    public override void BulletMovement(Vector3 forceDirection)
    {
        if(pc != null)
        {
            bulletRB.AddForce(forceDirection * bulletSpeed, ForceMode.Impulse);
        }
        else
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
    }
}

   
