using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : BaseBullet
{
    public int fireDamage;
    public float fireDuration;
    public float tickSpeed;
    public override void Start()
    {
        base.Start();
        this.bulletType = BulletTypes.FIRE;
        if (isPC)
        {
            BulletMovement(pc.GetPCShoot().forward);
        }
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

    public override void BulletMovement(Vector3 forceDirection)
    {
        bulletRB.AddForce(forceDirection * bulletSpeed, ForceMode.Impulse);
    }
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (collision.CompareTag("Enemies"))
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().SetBurning(fireDamage, fireDuration, tickSpeed);
            }
        }
    }
    public override void Die()
    {
        base.Die();
    }
}