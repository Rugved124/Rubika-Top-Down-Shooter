using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private PC pc;

    [SerializeField]
    private Transform respawner; 
    private void Start()
    {
        pc = FindObjectOfType<PC>();
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
