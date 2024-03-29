using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourChange : MonoBehaviour
{
    [SerializeField]
    private Image soulIndicator;
    void Update()
    {
        GetComponent<Image>().color = soulIndicator.color;
    }
}
