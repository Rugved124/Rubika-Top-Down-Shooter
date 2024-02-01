using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : BaseBullet
{

    public override void Start()
    {
        base.Start();
        this.bulletType = BulletTypes.DEFAULTAMMO;
        bulletRB = this.GetComponent<Rigidbody>();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void InitializeBullet()
    {
        base.InitializeBullet();
    }

    public override void BulletMovement()
    {
        base.BulletMovement();
        bulletRB.AddForce(pc.GetPCShoot().forward * bulletSpeed, ForceMode.Impulse);
    }
}
