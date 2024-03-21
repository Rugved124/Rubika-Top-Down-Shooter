using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    bool isMuted;
    void Start()
    {
        isMuted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopAudio()
    {
        if (!isMuted)
        {
            isMuted = true;
            AudioListener.pause = true;
        }
        else
        {
            isMuted = false;
            AudioListener.pause = false;
        }
    }
}
