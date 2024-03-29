using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    Vector3 startPos;
    Vector3 startRotation;
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
    public IEnumerator ShakeCamera(float duration, float magnitude, float timeBetweenEachShake, float speed)
    {
        float time = 0;
        float shakeTime = 0;
        if (!isCameraShakeCalled)
        {
            isCameraShakeCalled = true;
            startPos = transform.position;
            startRotation = transform.localEulerAngles;
        }
        while (time <= duration)
        {
            time += Time.deltaTime;
            Vector3 shake = new Vector3(Random.Range(shakeAmount.x,-shakeAmount.x), Random.Range(shakeAmount.y, -shakeAmount.y), Random.Range(shakeAmount.z, -shakeAmount.z));
            Vector3 rotation = new Vector3(Random.Range(rotationAmount.x,-rotationAmount.x), Random.Range(rotationAmount.y, -rotationAmount.y), Random.Range(rotationAmount.z, -rotationAmount.z));
            if (shakeTime >= timeBetweenEachShake)
            {
                shakeTime = 0;
                shake = new Vector3(Random.Range(shakeAmount.x, -shakeAmount.x), Random.Range(shakeAmount.y, -shakeAmount.y), Random.Range(shakeAmount.z, -shakeAmount.z));
                rotation = new Vector3(Random.Range(rotationAmount.x, -rotationAmount.x), Random.Range(rotationAmount.y, -rotationAmount.y), Random.Range(rotationAmount.z, -rotationAmount.z));
            }
            while(shakeTime < timeBetweenEachShake)
            {
                shakeTime += Time.deltaTime;
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, transform.localEulerAngles + (rotation * magnitude), speed);
                 transform.position = Vector3.Lerp(transform.position, transform.position + (shake * magnitude), speed);
            }
            duration -= Time.deltaTime;
            yield return null;
        }

        isCameraShakeCalled = false;
        transform.localEulerAngles = startRotation;
    }

    public bool IsCameraShaking() 
    {
        return isCameraShakeCalled;
    }
}
