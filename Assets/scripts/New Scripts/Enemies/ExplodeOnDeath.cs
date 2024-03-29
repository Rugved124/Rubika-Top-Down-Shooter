using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField]
    GameObject explosion;
    private void OnDestroy()
    {
        GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explode, 0.8f);
    }
}
