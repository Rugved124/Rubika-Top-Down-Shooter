using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ExplosiveShotgun : BaseBullet
{
    [SerializeField]
    private float visionConeAngle = 90f;

    [SerializeField]
    private float pelletOffset;

    [SerializeField]
    private GameObject pellets;

    [SerializeField]
    private int pelletCount;

    private Quaternion startingAngle = Quaternion.AngleAxis(-0, Vector3.up);
    private Quaternion stepAngle = Quaternion.AngleAxis(0, Vector3.up);
    public override void Start()
    {
        base.Start();
        startingAngle = Quaternion.AngleAxis(-visionConeAngle / 2, Vector3.up);
        stepAngle = Quaternion.AngleAxis(visionConeAngle / pelletCount, Vector3.up);
        this.bulletType = BulletTypes.FIREPOISON;
        BulletMovement(transform.position);
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
        Quaternion angle = transform.rotation * startingAngle;

        Vector3 direction = angle * Vector3.forward;

        //Vector3 pos = angle * (forceDirection + Vector3.forward).normalized;
        //pos = transform.position + pos;
        if (pc != null)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                Debug.DrawLine(transform.position + direction.normalized * pelletOffset, transform.position + direction.normalized * (bulletSpeed * bulletLifeTime), Color.red);
                Rigidbody currrentPellet = Instantiate(pellets, transform.position + direction.normalized, angle).GetComponent<Rigidbody>();
                currrentPellet.gameObject.GetComponent<ShotgunPellets>().SetBaseStats(bulletType, bulletLifeTime, bulletDamage, bulletRange);
                currrentPellet.gameObject.GetComponent<ShotgunPellets>().DidPCShotThis(isPC);
                currrentPellet.AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
                direction = stepAngle * direction;
                //pos = stepAngle * pos;
                if (i >= pelletCount)
                {
                    Die();
                }
            }
            

        }
        else
        {
            Die();
        }
    }
}
