using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RespawnManager : MonoBehaviour
{
    private PC pc;

    [SerializeField]
    private Transform respawner;

    
    private void Start()
    {
        pc = FindObjectOfType<PC>();

    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            if (pc != null)
            {
                pc.respawnPoint = respawner.position;
            }
        }    
    }
}
