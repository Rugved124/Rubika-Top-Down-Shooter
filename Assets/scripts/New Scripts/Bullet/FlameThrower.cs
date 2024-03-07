using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : BaseBullet
{
    public int fireDamage;
    [SerializeField]
    protected float visionConeAngle = 90f;

    Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);
    public override void Start()
    {
        base.Start();

    }

    public override void Update()
    {
        base.Update();
        RaycastHit hit;

        Quaternion angle = transform.rotation * startingAngle;

        Vector3 direction = angle * Vector3.forward;

        Vector3 pos = transform.position;

        for (int i = 0; i < (visionConeAngle / 5) + 1; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, bulletRange))
            {
                if (hit.collider.tag == "Enemies")
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage); 
                }
            }
            direction = stepAngle * direction;
        }
    }
    public override void Die()
    {
        base.Die();
    }
}
