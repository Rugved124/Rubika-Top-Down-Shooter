using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggercollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Collider>().enabled = false;
            StartCoroutine(TurnColliderToNormal());
        }
    }

    IEnumerator TurnColliderToNormal()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<PC>().GetComponent<Collider>().enabled = true;
    }
}
