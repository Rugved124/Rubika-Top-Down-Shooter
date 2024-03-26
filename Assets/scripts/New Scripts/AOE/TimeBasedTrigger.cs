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

    [SerializeField]
    private List<float> AOETimers = new List<float>();

    [SerializeField]
    bool canLoop;
    private void Awake()
    {
        aoeTimer = AOETimers[i];
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
            TriggerNextAOE();
        }
    }
    public void TriggerNextAOE()
    {
        if(i < aoe.Count)
        {
            aoe[i].SetActive(true);
            i++;
            if(i < aoe.Count)
            {
                aoeTimer = AOETimers[i];
            }
        }
    }
}
