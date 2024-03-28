using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEFeedBack : MonoBehaviour
{
    [SerializeField]
    float wait = 1f;

    [SerializeField]
    GameObject feedback;

    bool canSet;
    void Awake()
    {
        canSet = true;
        this.GetComponent<Collider>().enabled = false;
        if (feedback != null)
        {
            feedback.SetActive(true);
        }
    }
    void Update()
    {
        wait -= Time.deltaTime;
        if(wait < 0f)
        {
            if(canSet )
            {
                canSet = false;
                this.GetComponent<Collider>().enabled = true;
                if (feedback != null)
                {
                    feedback.SetActive(false);
                }
            }
        }
    }
}
