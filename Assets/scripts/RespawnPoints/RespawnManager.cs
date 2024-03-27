using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    private PC pc;

    [SerializeField]
    private Transform respawner;

    [SerializeField]
    List<GameObject> lastRoomLights;

    [SerializeField]
    private List<GameObject> currentRoomLights;

    [SerializeField]
    private List<GameObject> otherRoomCollider;

    [SerializeField]
    private List<GameObject> currentRoomCollider;

    bool setRespawn;
    private void Start()
    {
        pc = FindObjectOfType<PC>();
        setRespawn = false;
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && setRespawn)
        {
            setRespawn = true;
            if (pc != null)
            {
                SaveManager.SavePlayerStats(respawner.position, pc.currentHP, AmmoManager.instance.firstAmmoType, AmmoManager.instance.secondAmmoType, AmmoManager.instance.GetAmmoCount(), SceneManager.GetActiveScene().buildIndex);
                //ManagerEvents.playerData.Invoke(pc.currentHP, AmmoManager.instance.GetAmmoCount(), AmmoManager.instance.firstAmmoType.ToString(), AmmoManager.instance.secondAmmoType.ToString());
                foreach (GameObject go in lastRoomLights)
                {
                    go.SetActive(false);
                }
                foreach(GameObject go in currentRoomLights)
                {
                    go.SetActive(true);
                }

                foreach (GameObject go in currentRoomCollider)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in otherRoomCollider)
                {
                    go.SetActive(true);
                }

            }
        }    
    }
}
