using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    Vector3 startPos;
    float startXRotation;
    float startYRotation;
    [SerializeField]
    Vector3 shakeAmount;

    [SerializeField]
    Vector3 rotationAmount;
    bool isCameraShakeCalled;

    float shakeSpeed;
    bool canChangeShakeDirection;
    private void Awake()
    {
        isCameraShakeCalled = false;
        canChangeShakeDirection = true;
    }
    public IEnumerator ShakeCamera(float easeIN, float easeOut, float duration, float magnitude, float timeBetweenEachShake)
    {
        float time = 0;
        float shakeTime = 0;
        if (!isCameraShakeCalled)
        {
            isCameraShakeCalled=true;
            startPos = transform.position;
            startXRotation = transform.rotation.eulerAngles.x;
            startYRotation = transform.rotation.eulerAngles.y;
        }
        while (time <= duration)
        {
            time += Time.deltaTime;
            shakeTime += Time.deltaTime;
            if (shakeTime >= timeBetweenEachShake)
            {
                shakeTime = 0;
            }
            if(shakeTime < timeBetweenEachShake)
            {
                 transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, transform.localEulerAngles + shakeAmount, magnitude);
                 transform.position = Vector3.Lerp(transform.position, transform.position + shakeAmount, magnitude);
            }
            duration -= Time.deltaTime;
            yield return null;
        }
    }
}
