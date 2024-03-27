using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDamage : BaseBullet
{
    public override void Start()
    {
        base.Start();
        Destroy(gameObject, bulletDamage);
    }

    public override void Update()
    {
        base.Update();
    }
}
