using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : BaseBullet
{
    [SerializeField]
    GameObject explosion;

    public override void Start()
    {
        base.Start();
        BulletMovement(transform.forward);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }

    public override void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        base.Die();
    }

    public override void BulletMovement(Vector3 forceDirection)
    {
        base.BulletMovement(forceDirection);
        bulletRB.AddForce(forceDirection * bulletSpeed, ForceMode.Impulse);
    }
}
