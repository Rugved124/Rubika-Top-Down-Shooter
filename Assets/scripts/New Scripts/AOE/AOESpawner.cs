using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> aoe;

    [SerializeField]
    List<float> timer;

    private void Start()
    {
        foreach (GameObject a in aoe)
        {
            a.SetActive(false);
        }
    }
    private void Update()
    {
        
    }
    private void ChooseRandomAOE()
    {
        int i = Random.Range(0, aoe.Count - 1);
    }

}
