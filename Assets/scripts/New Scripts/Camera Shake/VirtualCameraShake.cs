using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator ShakeCamera(float shakeAmount, float duration, float speed)
    {
        CinemachineBasicMultiChannelPerlin cam_perlin = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        float time = 0;
        while (time < duration/2) 
        {
            cam_perlin.m_AmplitudeGain = Mathf.Lerp(0, shakeAmount,speed);
            time += Time.deltaTime;
            yield return null;
        }
        while (time >= duration/2 && time < duration) 
        {
            cam_perlin.m_AmplitudeGain = Mathf.Lerp( shakeAmount,0, speed);
            time += Time.deltaTime;
            yield return null;
        }
        cam_perlin.m_AmplitudeGain = 0;
    }

}
