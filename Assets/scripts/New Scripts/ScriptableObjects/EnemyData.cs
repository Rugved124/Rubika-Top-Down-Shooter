using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public  float visionConeAngle = 180;
    public float aggroRadius = 5f;

    public float attackRange = 10f;

    public int HitPoints = 100;

    public readonly Quaternion startingAngle = Quaternion.AngleAxis(0, Vector3.up);
    public readonly Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    public float enemyMoveSpeed = 5f;
    public float enemyRotationSpeed = 200f;

    public float enemyAttackingAngularSpeed = 0.4f;

    public float timeBetweenBullets = 0.5f;
    public AmmoManager.EquippedAmmoType bullet;

    public float allyDetectionRange;

    public float runAwayDistance;
}
