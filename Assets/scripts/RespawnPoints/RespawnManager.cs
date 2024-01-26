using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    GameObject PC;

    private void Start()
    {
        PC = GameObject.Find("PC");
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Backspace) && PCDie.pcDie.isDead)
        {
            
            RespawnThePC();
        }
    }
    void RespawnThePC()
    {
        PC.transform.position = PCRespawn.respawn.respawnPoint;
        PC.SetActive(true);
        PCHealth.instance.hitPoints = PCHealth.instance.maxHitPoints;
        PCDie.pcDie.isDead = false;
        Debug.Log("respawn");
        
        
    }
}
