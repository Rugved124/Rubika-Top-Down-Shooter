using UnityEngine;

public class EnemyConsumedState : MonoBehaviour
{
    EnemyMovement enemyMove;
    EnemyShooting enemyShooting;
    [SerializeField]
    private BulletType.bulletType enemysBulleteType;
    private void Start()
    {
        enemyMove = GetComponent<EnemyMovement>();
        enemyShooting = GetComponent<EnemyShooting>();
    }
    void Update()
    {

    }
    public void Consumed()
    {
        enemyMove.enabled = false;
        enemyShooting.enabled = false;
    }

    public void MoveAndSHoot()
    {
        enemyMove.enabled = true;
        enemyShooting.enabled = true;
    }
    public BulletType.bulletType GetEnemyBulletType()
    {
        return enemysBulleteType;
    }

}
