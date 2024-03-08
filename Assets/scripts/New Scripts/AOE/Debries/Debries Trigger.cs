using System.Collections.Generic;
using UnityEngine;

public class DebriesTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> debries;

    private void Awake()
    {
        foreach (GameObject debry in debries)
        {
            debry.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject debry in debries)
            {
                debry.SetActive(true);
            }
        }
    }
}
