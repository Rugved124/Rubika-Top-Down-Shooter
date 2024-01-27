using UnityEngine;

public class PCShield : MonoBehaviour
{
    [SerializeField]
    private int giveShieldPoints;
    void Update()
    {
        if (InputManager.instance.GetIfShieldIsPressed())
        {
            PCHealth.instance.SetShieldPoints(giveShieldPoints);
        }
    }
}
