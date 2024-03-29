using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEFeedBack : MonoBehaviour
{
    [SerializeField]
    float wait = 1f;

    [SerializeField]
    GameObject feedback;

    [SerializeField]
    GameObject visuals;

    bool canSet;

    float inGameFeedback;
    void Awake()
    {
        canSet = true;
        this.GetComponent<Collider>().enabled = false;
        inGameFeedback = wait;
        if(visuals != null)
        {
            visuals.SetActive(false);
        }
        if (feedback != null)
        {
            feedback.SetActive(true);
        }
    }
    void Update()
    {
        inGameFeedback -= Time.deltaTime;
        if(inGameFeedback < 0f)
        {
            if(canSet )
            {
                canSet = false;
                this.GetComponent<Collider>().enabled = true;
                if (feedback != null)
                {
                    feedback.SetActive(false);
                }
                if (visuals != null)
                {
                    visuals.SetActive(true);
                }
            }
        }
    }
}
