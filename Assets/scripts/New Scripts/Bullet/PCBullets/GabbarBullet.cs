using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabbarBullet : BaseBullet
{
    [SerializeField]
    private GameObject poisonAOE;

    [SerializeField]
    private LayerMask groundLayer;
    public override void Start()
    {
        base.Start();
        this.bulletType = BulletTypes.POISON;
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
        if (collision.tag != "Player" && collision.tag != "Ground" && collision.tag != "Shield" && collision.tag != "Enemies")
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit,groundLayer))
            {
                if (hit.collider != null)
                {
                    Instantiate(poisonAOE, hit.point, Quaternion.identity);
                    Die();
                }
            }

        }
    }
    public override void Die()
    {
        base.Die();
    }
}