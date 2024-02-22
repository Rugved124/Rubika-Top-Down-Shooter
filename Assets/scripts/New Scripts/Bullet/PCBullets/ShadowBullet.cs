using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBullet : BaseBullet
{
    [SerializeField]
    private int pierceCharges;

    [SerializeField]
    private GameObject currentContact;
    public override void Start()
    {
        base.Start();
        this.bulletType = BulletTypes.SLOW;
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
    public new void OnTriggerEnter(Collider collision)
    {

        if (pierceCharges <= 0 && collision != null)
        {
            Die();
        }
        if (collision.CompareTag("Shield") && pierceCharges > 0 && currentContact != collision.gameObject)
        {
            if (!collision.GetComponent<ShieldBehaviour>().isPC)
            {
                currentContact = collision.gameObject;
                collision.GetComponent<ShieldBehaviour>().TakeDamage(1);
                bulletDamage /= 2;
                pierceCharges--;
            }

        }
        if (collision.CompareTag("Enemies") && pierceCharges > 0 && currentContact != collision.gameObject)
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                currentContact = collision.gameObject;
                collision.GetComponent<Enemy>().TakeDamage(bulletDamage);
                pierceCharges--;
            }

        }

    }
    public override void Die()
    {
        base.Die();
    }
}
