using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : BaseBullet
{
    [SerializeField]
    GameObject explosion;

    Vector3 startPos;
    public override void Start()
    {
        base.Start();
        BulletMovement(transform.forward);
        startPos = transform.position;
    }

    public override void Update()
    {
        base.Update();
        if (Vector3.Distance(startPos, transform.position) >= bulletRange) 
        {
            Die();
        }
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
