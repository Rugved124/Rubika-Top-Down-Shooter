using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != null && collision.collider.tag != "Enemies" && collision.collider.tag != "EnemyBulletDefault")
        {
           Destroy(this.gameObject);
        }
    }
}
