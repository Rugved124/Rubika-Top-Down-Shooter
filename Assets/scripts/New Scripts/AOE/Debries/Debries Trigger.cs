using System.Collections.Generic;
using UnityEngine;

public class DebriesTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> debries;

    bool canHit;
    private void Awake()
    {
        canHit = true;
        foreach (GameObject debry in debries)
        {
            debry.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHit)
        {
            foreach (GameObject debry in debries)
            {
                debry.SetActive(true);
            }
            canHit = false;
        }
    }
}
