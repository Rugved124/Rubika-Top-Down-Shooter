using System.Collections.Generic;
using UnityEngine;

public class TimeBasedTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> aoe = new List<GameObject>();

    int i;
    [SerializeField]
    private float aoeTimer;
    [SerializeField]
    private float maxAoeTimer;
    private void Awake()
    {
        aoeTimer = maxAoeTimer;
        foreach (GameObject i in aoe) 
        {
            i.SetActive(false);
        }
    }
    private void Update()
    {
        if(i < aoe.Count)
        {
            aoeTimer -= Time.deltaTime;
        }
        if(aoeTimer <= 0f)
        {
            aoeTimer = maxAoeTimer;
            TriggerNextAOE();
        }
    }
    public void TriggerNextAOE()
    {
        if(i < aoe.Count)
        {
            aoe[i].SetActive(true);
            i++;
        }
    }
}
