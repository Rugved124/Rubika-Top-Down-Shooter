using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : BaseBullet
{
    [SerializeField]
    protected float visionConeAngle = 90f;

    Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    float timeBetweenTick;

    [SerializeField]
    float timeBetweenAmmo;

    [SerializeField]
    LayerMask obstacles;

    [SerializeField]
    private float timeBeforeFire = 1;

    float chargeTime;
    float lifeTime;
    public override void Start()
    {
        base.Start();
        timeBetweenTick = 1/ bulletRate;
        chargeTime = timeBeforeFire;
        Invoke("Die", bulletLifeTime);
    }

    public override void Update()
    {   
        chargeTime -= Time.deltaTime;
        if (!pc.isDead)
        {
            pc.slowMultiplier = 0.5f;
            transform.forward = pc.gameObject.transform.forward;
            transform.position = pc.GetPCShoot().position;
            if (chargeTime <= 0)
            {
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
                    Debug.DrawRay(pos, direction.normalized * bulletRange, Color.red);
                    if (Physics.Raycast(pos, direction, out hit, bulletRange, obstacles))
                    {
                        if (hit.collider.tag == "Enemies")
                        {
                            timeBetweenTick -= Time.deltaTime;
                            if (timeBetweenTick <= 0f)
                            {
                                timeBetweenTick = 1/bulletRate;
                                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
                            }
                        }
                        if (hit.collider.CompareTag("Boss"))
                        {
                            if (hit.collider.gameObject.GetComponent<BossHealth>().GetCurrentBulletType() == "FIRE")
                            {
                                timeBetweenTick -= Time.deltaTime;
                                if (timeBetweenTick <= 0f)
                                {
                                    timeBetweenTick = 1 / bulletRate;
                                    hit.collider.gameObject.GetComponent<BossHealth>().TakeDamage();
                                }
                            }
                        }
                        if (hit.collider.CompareTag("Shield"))
                        {
                            hit.collider.gameObject.GetComponent<ShieldBehaviour>().SetPoisonedForTime();
                        }
                    }
                    direction = stepAngle * direction;
                }
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
