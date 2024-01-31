using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : BaseBullet
{

    public override void Start()
    {
        base.Start();
        this.bulletType = BulletTypes.DEFAULTAMMO;
    }
}
