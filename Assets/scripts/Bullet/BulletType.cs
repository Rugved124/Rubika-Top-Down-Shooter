using UnityEngine;

public class BulletType : MonoBehaviour
{
    public static BulletType bulletState;
    public bulletType bulletTypeForShooting;
    [SerializeField]
    private float consumeRange;
    private bulletType firstBullet;
    private bulletType secondBullet;
    [SerializeField]
    private int firstBulletCount;
    [SerializeField]
    private int secondBulletCount;
    bool isConsuming;
    float consumeTime;
    [SerializeField]
    private float maxConsumeDuration;
    private EnemyConsumedState _enemy;
    private void Awake()
    {
        if (bulletState == null)
        {
            bulletState = this;
        }
        else {
            Destroy(this);
        }
    }
    private void Start()
    {
        firstBullet = bulletType.Default;
        secondBullet = bulletType.Default;
        isConsuming = false;
    }
    public enum bulletType
    {
        Default, Red, Blue, Green, RedGreen, BlueGreen, RedBlue
    }
    
    void Update()
    {
        Debug.Log(isConsuming);
        if(_enemy != null)
        {
            _enemy.MoveAndSHoot();
            _enemy = null;
        }
        if (firstBullet == bulletType.Default) bulletTypeForShooting = secondBullet;
        if (secondBullet == bulletType.Default) bulletTypeForShooting = firstBullet;
        if ((firstBullet == bulletType.Red && secondBullet == bulletType.Blue) || (firstBullet == bulletType.Blue && secondBullet == bulletType.Red)) bulletTypeForShooting = bulletType.RedBlue;
        if ((firstBullet == bulletType.Red && secondBullet == bulletType.Green) || (firstBullet == bulletType.Green && secondBullet == bulletType.Red)) bulletTypeForShooting = bulletType.RedGreen;
        if ((firstBullet == bulletType.Blue && secondBullet == bulletType.Green) || (firstBullet == bulletType.Green && secondBullet == bulletType.Blue)) bulletTypeForShooting = bulletType.BlueGreen;

        if (InputManager.instance.GetIfConsumeIsHeld() && !PCStatusEffects.instance.consumeIsDisabled)
        {
            var consumeRay = InputManager.instance.GetMousePosition() - transform.position;
            Debug.DrawRay(transform.position, transform.forward.normalized * consumeRange, Color.black);
            Debug.Log(transform.forward);
            RaycastHit hit;     
            Physics.Raycast(transform.position, transform.forward, out hit, consumeRange);
            if (hit.collider != null)
            {

                if ((hit.collider.tag == "Enemies" || hit.collider.tag == "Red" || hit.collider.tag == "Blue" || hit.collider.tag == "Green") && !isConsuming)
                {
                    PCController.instance.enabled = false;
                    transform.forward = transform.forward;
                    consumeTime = Time.time;
                    isConsuming = true;
                    consumeRay = hit.collider.transform.position;
                }
                if (hit.collider.tag == "Enemies")
                {
                    var enemy = hit.collider.GetComponent<EnemyConsumedState>();
                    enemy.Consumed();
                    _enemy =  enemy;
                    if (Time.time - consumeTime >= maxConsumeDuration)
                    {
                        firstBullet = secondBullet;
                        secondBullet = enemy.GetEnemyBulletType();
                        Destroy(hit.collider.gameObject);
                    }
                }
                    if (hit.collider.tag == "Red" && Time.time - consumeTime >= maxConsumeDuration)
                {
                    firstBullet = secondBullet;
                    secondBullet = bulletType.Red;
                    Destroy(hit.collider.gameObject);
                }
                if (hit.collider.tag == "Green" && Time.time - consumeTime >= maxConsumeDuration)
                {
                    firstBullet = secondBullet;
                    secondBullet = bulletType.Green;
                    Destroy(hit.collider.gameObject);
                }
                if (hit.collider.tag == "Blue" && Time.time - consumeTime >= maxConsumeDuration)
                {
                    firstBullet = secondBullet;
                    secondBullet = bulletType.Blue;
                    Destroy(hit.collider.gameObject);
                }
                
            }
            else 
            {
                isConsuming = false;
            }
        }
        else if (!InputManager.instance.GetIfConsumeIsHeld())
        {
            PCController.instance.enabled = true;
            isConsuming = false;
        }
    }
    public bulletType GetBulletTypeForShooting()
    {
        return bulletTypeForShooting;
    }
}


