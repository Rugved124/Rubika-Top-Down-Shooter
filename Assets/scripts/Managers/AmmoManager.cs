using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance;
    [SerializeField]
    List<GameObject> bulletTypes = new List<GameObject>();
    [SerializeField]
    GameObject bulletToSpawn;
    PC pc;
    [SerializeField]
    int ammoCount;
    bool canShoot;

    [SerializeField]
    private GameObject shield;

    public GameObject currentShield;
    public enum EquippedAmmoType
    {
        DEFAULTAMMO,
        FIRE,
        FIREFIRE,
        POISON,
        POISONPOSION,
        SLOW,
        SLOWSLOW,
        FIREPOISON,
        POISONFIRE,
        SLOWFIRE,
        FIRESLOW,
        SLOWPOISON,
        POISONSLOW,
        SHIELD,
        SHIELDSHIELD
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }
        else
        {
            Destroy(this);
        }
    }
    public EquippedAmmoType currentAmmoType;

    public EquippedAmmoType firstAmmoType;
    public EquippedAmmoType secondAmmoType;
    private void Start()
    {
        currentAmmoType = EquippedAmmoType.DEFAULTAMMO;
        pc = FindObjectOfType<PC>();
        ChangeAmmoType();
        canShoot = true;
    }
    private void Update()
    {
        if(InputManager.instance.IsMousePressed() && pc != null) 
        {
            if(bulletToSpawn != null && canShoot && ammoCount > 0)
            {
                
                canShoot = false;
                StartCoroutine(bulletFireRate());
                GameObject bulletShot = Instantiate(bulletToSpawn, pc.GetPCShoot().position, Quaternion.identity);
                bulletShot.GetComponent<BaseBullet>().DidPCShotThis(true);
                ammoCount--;
            }
        }
        if(ammoCount <= 0)
        {
            ChangeEquippedAmmo(EquippedAmmoType.DEFAULTAMMO);
        }
        if(currentShield != null && pc != null)
        {
            currentShield.GetComponent<ShieldBehaviour>().FollowSpawner(pc.transform);
        }
    }
    public void ChangeEquippedAmmo(EquippedAmmoType newAmmo)
    {
        currentAmmoType = newAmmo;
        ChangeAmmoType();
    }
    public void ChangeAmmoType()
    {
        switch (currentAmmoType)
        {
            case EquippedAmmoType.DEFAULTAMMO:

                GetBulletObject(BaseBullet.BulletTypes.DEFAULTAMMO);

                break;
            case EquippedAmmoType.POISON:
                GetBulletObject(BaseBullet.BulletTypes.POISON);
                break;
            case EquippedAmmoType.SLOW:
                GetBulletObject(BaseBullet.BulletTypes.SLOW);
                break;
            case EquippedAmmoType.FIRE:
                GetBulletObject(BaseBullet.BulletTypes.FIRE);
                break;
            case EquippedAmmoType.SHIELD:
                GetBulletObject(BaseBullet.BulletTypes.DEFAULTAMMO);
                currentShield = Instantiate(shield, transform.position, Quaternion.identity);
                currentShield.transform.forward = transform.forward;
                currentShield.GetComponent<ShieldBehaviour>().IsPCShield(true);
                break;
        }
    }
    public void GetBulletObject(BaseBullet.BulletTypes currentBulletType)
    {
     
        foreach (GameObject bullet in bulletTypes)
        {
            if (bullet.GetComponent<BaseBullet>() != null)
            {
                if (bullet.GetComponent<BaseBullet>().bulletType == currentBulletType)
                {
                    bulletToSpawn = bullet;
                    ammoCount = bulletToSpawn.GetComponent<BaseBullet>().ammoCount;
                    return;
                }
            }
        }
   
    }
    IEnumerator bulletFireRate()
    {
        yield return new WaitForSeconds(1/bulletToSpawn.GetComponent<BaseBullet>().bulletRate);
        canShoot = true;
    }

}
