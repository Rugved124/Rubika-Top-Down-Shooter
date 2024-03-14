using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RespawnManager : MonoBehaviour
{
    private PC pc;

    [SerializeField]
    private Transform respawner;

    [SerializeField]
    List<GameObject> lastRoomLights;

    [SerializeField]
    private List<GameObject> currentRoomLights;
    
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
                foreach (GameObject go in lastRoomLights)
                {
                    go.SetActive(false);
                }
                foreach(GameObject go in currentRoomLights)
                {
                    go.SetActive(true);
                }
            }
        }    
    }
}
