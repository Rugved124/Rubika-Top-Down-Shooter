using UnityEngine;

public class BulletType : MonoBehaviour
{
    public static BulletType bulletState;
    public bulletType bulletTypeForShooting;
    [SerializeField]
    private float consumeRange;
    private bulletType firstBullet;
    private bulletType secondBullet;

    public int bulletCountForShooting;
    bool isConsuming;
    float consumeTime;
    [SerializeField]
    private float maxConsumeDuration;

    private float consumedTime;
    [SerializeField]
    private float consumeCoolDown;

    private EnemyConsumedState _enemy;

    [SerializeField]
    private int maxRed;
    [SerializeField]
    private int maxGreen;
    [SerializeField]
    private int maxBlue;

    public int firstBulletCount, secondBulletCount;
    bool isShootingAllowed;
    private void Awake()
    {
        if (bulletState == null)
        {
            bulletState = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        firstBullet = bulletType.Default;
        secondBullet = bulletType.Default;
        isConsuming = false;
        isShootingAllowed = true;
        consumedTime = -consumeCoolDown;
    }
    public enum bulletType
    {
        Default, Red, Blue, Green, RedGreen, BlueGreen, RedBlue
    }

    void Update()
    {
        Debug.Log(firstBullet);
        Debug.Log(secondBullet);
        if (_enemy != null)
        {
            _enemy.MoveAndSHoot();
            _enemy = null;
        }
        if (firstBullet == bulletType.Default) bulletTypeForShooting = secondBullet;
        if (secondBullet == bulletType.Default) bulletTypeForShooting = firstBullet;
        if (firstBullet == secondBullet) bulletTypeForShooting = secondBullet;
        if (secondBulletCount <= 0)
        {
            secondBullet = bulletType.Default;
            secondBulletCount = 10;
        }
        if (firstBulletCount <= 0)
        {
            firstBullet = bulletType.Default;
            firstBulletCount = 10;
        }
        if ((firstBullet == bulletType.Red && secondBullet == bulletType.Blue) || (firstBullet == bulletType.Blue && secondBullet == bulletType.Red)) bulletTypeForShooting = bulletType.RedBlue;
        if ((firstBullet == bulletType.Red && secondBullet == bulletType.Green) || (firstBullet == bulletType.Green && secondBullet == bulletType.Red)) bulletTypeForShooting = bulletType.RedGreen;
        if ((firstBullet == bulletType.Blue && secondBullet == bulletType.Green) || (firstBullet == bulletType.Green && secondBullet == bulletType.Blue)) bulletTypeForShooting = bulletType.BlueGreen;

        if (InputManager.instance.GetIfConsumeIsHeld() && Time.time - consumedTime >= consumeCoolDown)
        {
            isShootingAllowed = false;
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
                }
                if (hit.collider.tag == "Enemies")
                {
                    var enemy = hit.collider.GetComponent<EnemyConsumedState>();
                    enemy.Consumed();
                    _enemy = enemy;
                    if (Time.time - consumeTime >= maxConsumeDuration)
                    {
                        firstBullet = secondBullet;
                        firstBulletCount = secondBulletCount;
                        secondBullet = enemy.GetEnemyBulletType();
                        secondBulletCount = SetBulletCount(secondBullet);
                        Destroy(hit.collider.gameObject);
                        consumedTime = Time.time;
                    }
                }
                if (hit.collider.tag == "Red" && Time.time - consumeTime >= maxConsumeDuration)
                {
                    firstBullet = secondBullet;
                    firstBulletCount = secondBulletCount;
                    secondBullet = bulletType.Red;
                    secondBulletCount = SetBulletCount(secondBullet);
                    Destroy(hit.collider.gameObject);
                    consumedTime = Time.time;
                }
                if (hit.collider.tag == "Green" && Time.time - consumeTime >= maxConsumeDuration)
                {
                    firstBullet = secondBullet;
                    firstBulletCount = secondBulletCount;
                    secondBullet = bulletType.Green;
                    secondBulletCount = SetBulletCount(secondBullet);
                    Destroy(hit.collider.gameObject);
                    consumedTime = Time.time;
                }
                if (hit.collider.tag == "Blue" && Time.time - consumeTime >= maxConsumeDuration)
                {
                    firstBullet = secondBullet;
                    firstBulletCount = secondBulletCount;
                    secondBullet = bulletType.Blue;
                    secondBulletCount = SetBulletCount(secondBullet);
                    Destroy(hit.collider.gameObject);
                    consumedTime = Time.time;
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
            isShootingAllowed = true;
        }
    }
    public bulletType GetBulletTypeForShooting()
    {
        return bulletTypeForShooting;
    }
    public void CountBulletShot()
    {
        if (firstBullet == bulletType.Default)
        {
            secondBulletCount--;
            firstBulletCount = 10;
        }
        if (secondBullet == bulletType.Default)
        {
            firstBulletCount--;
            secondBulletCount = 10;
        }
        if (firstBullet == bulletType.Default && secondBullet == bulletType.Default)
        {
            firstBulletCount = 10; secondBulletCount = 10;
        }
        if (secondBullet != bulletType.Default && firstBullet != bulletType.Default)
        {
            firstBulletCount--;
            secondBulletCount--;
        }
    }
    public int SetBulletCount(bulletType secondBullet)
    {
        if (secondBullet == bulletType.Default) { return 10; }
        if (secondBullet == bulletType.Red) { return maxRed; }
        if (secondBullet == bulletType.Blue) { return maxBlue; }
        if (secondBullet == bulletType.Green) { return maxGreen; }
        else return 0;
    }

    public bool canShoot()
    {
        if (firstBulletCount > 0 && secondBulletCount > 0 && isShootingAllowed) { return true; }
        else { return false; }
    }
    public void LostConsumedSoul()
    {
        firstBullet = bulletType.Default;
        secondBullet = bulletType.Default;
    }
    public int GiveFirstBulletCount()
    {
        if (firstBullet == bulletType.Default) { return 10; }
        if (firstBullet == bulletType.Red) { return maxRed; }
        if (firstBullet == bulletType.Blue) { return maxBlue; }
        if (firstBullet == bulletType.Green) { return maxGreen; }
        else return 0;
    }
    public int GiveSecondBulletCount()
    {
        if (secondBullet == bulletType.Default) { return 10; }
        if (secondBullet == bulletType.Red) { return maxRed; }
        if (secondBullet == bulletType.Blue) { return maxBlue; }
        if (secondBullet == bulletType.Green) { return maxGreen; }
        else return 0;
    }
    public int GetFirstbulletCount()
    {
        return firstBulletCount;
    }
    public int GetSecondbulletCount()
    {
        return secondBulletCount;
    }
    public bulletType GetFirstBullet()
    {
        return firstBullet;
    }
}


