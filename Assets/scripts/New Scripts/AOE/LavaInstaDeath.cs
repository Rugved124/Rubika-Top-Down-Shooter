using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaInstaDeath : MonoBehaviour
{
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(FindObjectOfType<PC>() != null)
            {
                FindObjectOfType<PC>().currentHP = 0;
            }
        }
    }
}
