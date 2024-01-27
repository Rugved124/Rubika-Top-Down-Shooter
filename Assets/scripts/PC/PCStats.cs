using UnityEditor.Rendering;
using UnityEngine;

public class PCStats : MonoBehaviour
{
    [SerializeField]
    public float playerMoveSpeed;
    protected int maxHitPoints = 100;
    protected int hitPoints;
    [SerializeField]
    protected int shieldPoints;
}
