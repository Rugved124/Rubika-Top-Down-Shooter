using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField]
    GameObject explosion;
    public void Explode()
    {
        GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explode, 1f);
    }
    public void InvokeExplosion()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("Explode", 0.8f);
    }
}
