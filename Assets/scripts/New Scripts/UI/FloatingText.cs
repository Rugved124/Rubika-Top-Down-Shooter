using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 3f;

    [SerializeField]
    Vector3 offset;

    [SerializeField]
    Vector3 randomizeVector = new Vector3(0.5f, 0, 0);

    [SerializeField]
    TMP_Text damageText;
    void Start()
    {
        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeVector.x, randomizeVector.x), Random.Range(-randomizeVector.y, randomizeVector.y), Random.Range(-randomizeVector.z, randomizeVector.z));
        Destroy(gameObject, lifeTime);
    }
    private void LateUpdate()
    {

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void SetDamageNumber(int damage)
    {
        damageText.text = damage.ToString();
    }
}
