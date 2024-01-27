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

        if (firstBullet == bulletType.Default) bulletTypeForShooting = secondBullet;
        if (secondBullet == bulletType.Default) bulletTypeForShooting = firstBullet;
        if ((firstBullet == bulletType.Red && secondBullet == bulletType.Blue) || (firstBullet == bulletType.Blue && secondBullet == bulletType.Red)) bulletTypeForShooting = bulletType.RedBlue;
        if ((firstBullet == bulletType.Red && secondBullet == bulletType.Green) || (firstBullet == bulletType.Green && secondBullet == bulletType.Red)) bulletTypeForShooting = bulletType.RedGreen;
        if ((firstBullet == bulletType.Blue && secondBullet == bulletType.Green) || (firstBullet == bulletType.Green && secondBullet == bulletType.Blue)) bulletTypeForShooting = bulletType.BlueGreen;
        if (InputManager.instance.GetIfConsumeIsHeld())
        {
            Debug.DrawRay(transform.position, (InputManager.instance.GetMousePosition() - transform.position).normalized * consumeRange, Color.black);
            Debug.Log("Consume");
            RaycastHit hit;
            Physics.Raycast(transform.position, InputManager.instance.GetMousePosition() - transform.position, out hit, consumeRange);
            if (hit.collider != null)
            {
                if ((hit.collider.tag == "Enemies" || hit.collider.tag == "Red" || hit.collider.tag == "Blue" || hit.collider.tag == "Green") && !isConsuming)
                {
                    PCController.instance.enabled = false;
                    transform.forward = transform.forward;
                    consumeTime = Time.time;
                    isConsuming = true;
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


