using JetBrains.Annotations;
using UnityEngine;

public class BulletType : MonoBehaviour
{
    public static BulletType bulletState;
    public bulletType bulletTypeForShooting;
    
    private bulletType previousBullet;

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
    public enum bulletType
    {
        Default, Red, Blue, Green, RedGreen, BlueGreen, RedBlue
    }
    
    void Update()
    {
       // Debug.Log(bulletTypeForShooting);
    }
    
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Red" && previousBullet == bulletType.Default || collider.tag == "Red" && previousBullet == bulletType.Red)
        {
            bulletTypeForShooting = bulletType.Red;
            Destroy (collider.gameObject);
            previousBullet = bulletType.Red;
        }
        if (collider.tag == "Green" && previousBullet == bulletType.Default || collider.tag == "Green" && previousBullet == bulletType.Green)
        {
            bulletTypeForShooting = bulletType.Green;
            Destroy(collider.gameObject);
            previousBullet = bulletType.Green;
        }
        if (collider.tag == "Blue" && previousBullet == bulletType.Default || collider.tag == "Blue" && previousBullet == bulletType.Blue)
        {
            bulletTypeForShooting = bulletType.Blue;
            Destroy(collider.gameObject);
            previousBullet = bulletType.Blue;
        }
        if (collider.tag == "Red" && previousBullet == bulletType.Blue || collider.tag == "Blue" && previousBullet == bulletType.Red)
        {
            bulletTypeForShooting = bulletType.RedBlue;
            if (collider.tag == "Red") previousBullet = bulletType.Red;
            if (collider.tag == "Blue") previousBullet = bulletType.Blue;
            Destroy(collider.gameObject);
            

        }
        if (collider.tag == "Blue" && previousBullet == bulletType.Green || collider.tag == "Green" && previousBullet == bulletType.Blue)
        {
            bulletTypeForShooting = bulletType.BlueGreen;
            if (collider.tag == "Blue") previousBullet = bulletType.Blue;
            if (collider.tag == "Green") previousBullet = bulletType.Green;
            Destroy(collider.gameObject);
        }
        if (collider.tag == "Red" && previousBullet == bulletType.Green || collider.tag == "Green" && previousBullet == bulletType.Red)
        {
            bulletTypeForShooting = bulletType.RedGreen;
            if (collider.tag == "Red") previousBullet = bulletType.Red;
            if (collider.tag == "Green") previousBullet = bulletType.Green;
            Destroy(collider.gameObject);

        }
    }
    public bulletType GetBulletTypeForShooting()
    {
        return bulletTypeForShooting;
    }
}


