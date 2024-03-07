using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityProjectile : BaseBullet
{
    [SerializeField]
    private GameObject gravityAOE;

    public override void Start()
    {
        base.Start();
        this.bulletType = BulletTypes.SLOWPOISON;
        if (isPC)
        {
            BulletMovement(pc.GetPCShoot().forward);
        }
        //bulletRB = this.GetComponent<Rigidbody>();

    }
    public override void Update()
    {
        base .Update();
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
        if (collision.tag != "Player" && collision.tag != "Ground" && collision.tag != "Shield" && collision.tag != "Enemies")
        {
            Die();
        }
    }
    public override void Die()
    {
        Instantiate(gravityAOE, transform.position, Quaternion.identity);
        base.Die();
    }
}