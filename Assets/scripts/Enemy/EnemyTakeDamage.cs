using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullets")
        {
            var bulletDamage = collision.gameObject.GetComponent<DefaultBulletDamage>();
            EnemyHealth.instance.TakeDamage(bulletDamage.GetBulletDamage());

        }
    }
}
