using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != null && collision.collider.tag != "Player" && collision.collider.tag != "Bullets")
        {
           Destroy(this.gameObject);
        }
    }
}
