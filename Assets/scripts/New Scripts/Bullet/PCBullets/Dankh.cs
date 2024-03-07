using UnityEngine;

public class Dankh : BaseBullet
{

    public override void Start()
    {
        base.Start();
        BulletMovement(transform.forward);
    }
    public override void Update()
    {
        base.Update();
    }

    public override void BulletMovement(Vector3 forceDirection)
    {
        base.BulletMovement(forceDirection);
        bulletRB.AddForce(forceDirection *  bulletSpeed, ForceMode.Impulse);
    }
    public override void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "Shield" && collision.tag != "Player" && collision.tag != "Enemies" && collision.tag != "Souls")
        {
            Die();
        }
        if (isPC)
        {
            if (collision.CompareTag("Shield"))
            {
                if (collision.GetComponent<ShieldBehaviour>() != null && !collision.GetComponent<ShieldBehaviour>().isPC)
                {
                    collision.GetComponent<ShieldBehaviour>().SetPoisonedForTime();
                    Die();
                }
            }
            if (collision.CompareTag("Enemies"))
            {
                if (collision.GetComponent<Enemy>() != null)
                {
                    collision.GetComponent<Enemy>().SetPoisonedForTime();
                    Die();
                }
            }
        }
    }
}
