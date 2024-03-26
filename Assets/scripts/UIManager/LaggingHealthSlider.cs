using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LaggingHealthSlider : MonoBehaviour
{
    [SerializeField]
    Slider healthSlider;

    [SerializeField]
    float reductionWait = 1f;

    bool canChange;
    public void Start()
    {
        canChange = true;
        this.GetComponent<Slider>().maxValue = healthSlider.maxValue;
        this.GetComponent<Slider>().value = healthSlider.maxValue;
    }
    public void Update()
    {
        if(this.GetComponent<Slider>().value != healthSlider.value)
        {
            if (canChange)
            {
                canChange = false;
                StartCoroutine(EaseBarValue());
            }
        }
    }

    IEnumerator EaseBarValue()
    {
        yield return new WaitForSeconds(reductionWait);
        while(this.GetComponent<Slider>().value != healthSlider.value) 
        {
            this.GetComponent<Slider>().value = Mathf.Lerp(this.GetComponent<Slider>().value, healthSlider.value, 0.005f);
            yield return null;
        }
        canChange = true;
    }
}
