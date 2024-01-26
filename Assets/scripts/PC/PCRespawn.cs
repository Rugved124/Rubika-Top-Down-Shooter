using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRespawn : MonoBehaviour
{
    public static PCRespawn respawn;
    public Vector3 respawnPoint;
    public bool isRespawnActive;
    
    private void Awake()
    {
        if (respawn == null)
        {
            respawn = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RespawnPoint")
        {
            isRespawnActive = true;
            respawnPoint = other.transform.position;
            
        }
    }
    

}