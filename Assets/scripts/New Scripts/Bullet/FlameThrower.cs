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

    [SerializeField]
    float timeBetweenTick;

    [SerializeField]
    float timeBetweenAmmo;
    public override void Start()
    {
        base.Start();
        timeBetweenTick = 0.2f;
    }

    public override void Update()
    {   
        if(!pc.isDead)
        {
            pc.slowMultiplier = 0.3f;
            transform.forward = pc.gameObject.transform.forward;
            transform.position = pc.GetPCShoot().position;

            timeBetweenAmmo -= Time.deltaTime;
            if (timeBetweenAmmo <= 0f)
            {
                timeBetweenAmmo = 0.1f;
                AmmoManager.instance.ReduceAmmoCount(1);
            }
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
                        timeBetweenTick -= Time.deltaTime;
                        if (timeBetweenTick <= 0f)
                        {
                            timeBetweenTick = 0.2f;
                            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
                        }

                    }
                }
                direction = stepAngle * direction;
            }
        }
        if (Input.GetMouseButtonUp(0) || pc.isDead || AmmoManager.instance.GetAmmoCount() <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        if(!pc.isDead)
        {
            pc.slowMultiplier = 1f;
        }
        base.Die();
    }
}
