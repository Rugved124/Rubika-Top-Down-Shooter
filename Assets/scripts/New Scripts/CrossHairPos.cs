using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairPos : MonoBehaviour
{
    [SerializeField]
    RectTransform crosshair;

    [SerializeField]
    RectTransform canvas_rect;
    Camera cam;

    [SerializeField]
    Canvas _canvas;

    PC pc;
    public void Start()
    {
        pc = FindObjectOfType<PC>();
        cam = Camera.main;
    }
    private void Update()
    {
        Vector3 mousePos = new Vector3(InputManager.instance.GetMousePosition().x, pc.transform.position.y, InputManager.instance.GetMousePosition().z);
        if(Vector3.Distance(pc.transform.position, mousePos) >= AmmoManager.instance.currentRange)
        {
            if (!pc.isDead && AmmoManager.instance.GetCurrentAmmoType() != AmmoManager.EquippedAmmoType.SLOWSLOW)
            {
                transform.position = pc.transform.position + (pc.transform.forward * AmmoManager.instance.currentRange);
            }
            else
            {
                if (!pc.isDead && AmmoManager.instance.GetCurrentAmmoType() == AmmoManager.EquippedAmmoType.SLOWSLOW)
                {
                    transform.position = mousePos;
                }
            }
        }
        else
        {
            transform.position = mousePos;
        }
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_rect, cam.WorldToScreenPoint(transform.position),_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam, out pos);
        crosshair.anchoredPosition = pos;
    }
}
