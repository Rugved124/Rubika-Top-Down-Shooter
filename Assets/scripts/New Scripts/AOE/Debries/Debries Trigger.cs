using System.Collections.Generic;
using UnityEngine;

public class DebriesTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> debries;

    bool canTrigger;
    private void Awake()
    {
        canTrigger = true;
        foreach (GameObject debry in debries)
        {
            debry.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTrigger)
        {
            foreach (GameObject debry in debries)
            {
                debry.SetActive(true);
            }
            canTrigger = false;
        }
    }
}
