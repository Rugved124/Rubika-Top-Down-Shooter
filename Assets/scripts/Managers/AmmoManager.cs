using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private GameObject halfShield;

    [SerializeField]
    private GameObject fullShield;

    public GameObject currentShield;

    [SerializeField]
    private Slider firstAmmo;
    [SerializeField]
    private Slider secondAmmo;

    public Image firstAmmoColor, secondAmmoColor;

    public float currentRange {  get; private set; }
    public enum EquippedAmmoType
    {
        DEFAULTAMMO,
        FIRE,
        FIREFIRE,
        POISON,
        POISONPOISON,
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
        firstAmmo.maxValue = secondAmmo.maxValue = ammoCount;
    }
    private void Update()
    {
        currentRange = bulletToSpawn.GetComponent<BaseBullet>().bulletRange;
        if(firstAmmoType == EquippedAmmoType.DEFAULTAMMO || firstAmmoType == EquippedAmmoType.SHIELD)
        {
            firstAmmo.value = firstAmmo.maxValue;
        }
        else
        {
            firstAmmo.value = ammoCount;

        }
        if (secondAmmoType == EquippedAmmoType.DEFAULTAMMO || secondAmmoType == EquippedAmmoType.SHIELD)
        {
            secondAmmo.value = firstAmmo.maxValue;
        }
        else
        {
            secondAmmo.value = ammoCount;
        }
        switch (firstAmmoType)
        {
            case EquippedAmmoType.DEFAULTAMMO:
                firstAmmoColor.color = Color.clear;
                break;
            case EquippedAmmoType.FIRE:
                firstAmmoColor.color = Color.red;
                break;
            case EquippedAmmoType.SLOW:
                firstAmmoColor.color = Color.blue;
                break;
            case EquippedAmmoType.POISON:
                firstAmmoColor.color = Color.green;
                break;
            case EquippedAmmoType.SHIELD:
                firstAmmoColor.color = Color.yellow;
                break;
        }
        switch (secondAmmoType)
        {
            case EquippedAmmoType.DEFAULTAMMO:
                secondAmmoColor.color = Color.clear;
                break;
            case EquippedAmmoType.FIRE:
                secondAmmoColor.color = Color.red;
                break;
            case EquippedAmmoType.SLOW:
                secondAmmoColor.color = Color.blue;
                break;
            case EquippedAmmoType.POISON:
                secondAmmoColor.color = Color.green;
                break;
            case EquippedAmmoType.SHIELD:
                secondAmmoColor.color = Color.yellow;
                break;
        }
        if (currentShield != null)
        {
            if(firstAmmoType != EquippedAmmoType.SHIELD && secondAmmoType != EquippedAmmoType.SHIELD)
            {
                Destroy(currentShield);
            }
        }
        if(InputManager.instance.IsMousePressed() && !pc.isDead && !pc.isDashing) 
        {
            if(bulletToSpawn != null && canShoot && ammoCount > 0)
            {
                
                canShoot = false;
                StartCoroutine(bulletFireRate());
                GameObject bulletShot = Instantiate(bulletToSpawn, pc.GetPCShoot().position, pc.GetPCShoot().rotation);
                bulletShot.GetComponent<BaseBullet>().DidPCShotThis(true);
                ammoCount--;
            }
        }
        if(ammoCount <= 0)
        {
            ResetEquippedAmmo();
        }
        if(currentShield != null && pc != null)
        {
            currentShield.GetComponent<ShieldBehaviour>().FollowSpawner(pc.transform);
        }
    }
    public void ChangeEquippedAmmo(EquippedAmmoType newAmmo)
    {

        firstAmmoType = secondAmmoType;
        secondAmmoType = newAmmo;
        if(secondAmmoType == EquippedAmmoType.SHIELD)
        {
            if(firstAmmoType != EquippedAmmoType.SHIELD)
            {
                if (currentShield == null)
                {
                    currentShield = Instantiate(halfShield, transform.position, Quaternion.identity);
                    currentShield.transform.forward = transform.forward;
                    currentShield.GetComponent<ShieldBehaviour>().IsPCShield(true);
                }
                else
                {
                    currentShield.GetComponent<ShieldBehaviour>().ResetShield();
                }
            }
            else
            {
                if (currentShield == null)
                {
                    currentShield = Instantiate(fullShield, transform.position, Quaternion.identity);
                    currentShield.transform.forward = -transform.forward;
                    currentShield.GetComponent<ShieldBehaviour>().IsPCShield(true);
                }
                else
                {
                    Destroy(currentShield);
                    currentShield = Instantiate(fullShield, transform.position, Quaternion.identity);
                    currentShield.transform.forward = -transform.forward;
                    currentShield.GetComponent<ShieldBehaviour>().IsPCShield(true);
                }
            }

        }
        if (firstAmmoType == EquippedAmmoType.DEFAULTAMMO || secondAmmoType == EquippedAmmoType.DEFAULTAMMO)
        {
            if (firstAmmoType == EquippedAmmoType.DEFAULTAMMO)
            {
                currentAmmoType = secondAmmoType;
            }
            else
            {
                currentAmmoType = firstAmmoType;
            }
        }
        if (firstAmmoType == EquippedAmmoType.SHIELD|| secondAmmoType == EquippedAmmoType.SHIELD)
        {
            if (firstAmmoType == EquippedAmmoType.SHIELD && secondAmmoType == EquippedAmmoType.SHIELD)
            {
                currentAmmoType = EquippedAmmoType.SHIELDSHIELD;
            }
            else
            {
                if(firstAmmoType != EquippedAmmoType.SHIELD)
                {
                    currentAmmoType = firstAmmoType;
                }
                else
                {
                    currentAmmoType = secondAmmoType;
                }
            }
        }
        switch (firstAmmoType)
        {
            case EquippedAmmoType.FIRE:
                if (secondAmmoType == EquippedAmmoType.FIRE)
                {
                    currentAmmoType = EquippedAmmoType.FIREFIRE;
                }
                if (secondAmmoType == EquippedAmmoType.POISON)
                {
                    currentAmmoType = EquippedAmmoType.FIREPOISON;
                }
                if (secondAmmoType == EquippedAmmoType.SLOW)
                {
                    currentAmmoType = EquippedAmmoType.FIRESLOW;
                }
                if (secondAmmoType == EquippedAmmoType.SHIELD)
                {
                    currentAmmoType = firstAmmoType;
                }

                break;
            case EquippedAmmoType.POISON:
                if (secondAmmoType == EquippedAmmoType.FIRE)
                {
                    currentAmmoType = EquippedAmmoType.POISONFIRE;
                }
                if (secondAmmoType == EquippedAmmoType.POISON)
                {
                    currentAmmoType = EquippedAmmoType.POISONPOISON;
                }
                if (secondAmmoType == EquippedAmmoType.SLOW)
                {
                    currentAmmoType = EquippedAmmoType.POISONSLOW;
                }
                if (secondAmmoType == EquippedAmmoType.SHIELD)
                {
                    currentAmmoType = firstAmmoType;
                }
                break;
            case EquippedAmmoType.SLOW:
                if (secondAmmoType == EquippedAmmoType.FIRE)
                {
                    currentAmmoType = EquippedAmmoType.SLOWFIRE;
                }
                if (secondAmmoType == EquippedAmmoType.POISON)
                {
                    currentAmmoType = EquippedAmmoType.SLOWPOISON;
                }
                if (secondAmmoType == EquippedAmmoType.SLOW)
                {
                    currentAmmoType = EquippedAmmoType.SLOWSLOW;
                }
                if (secondAmmoType == EquippedAmmoType.SHIELD)
                {
                    currentAmmoType = firstAmmoType;
                }
                break;
            case EquippedAmmoType.SHIELD:
                if (secondAmmoType != EquippedAmmoType.SHIELD)
                {
                    currentAmmoType = secondAmmoType;
                }
                else
                {
                    currentAmmoType = EquippedAmmoType.SHIELDSHIELD;
                }
                break;
        }
        ChangeAmmoType();
    }

    void ResetEquippedAmmo()
    {
        firstAmmoType = EquippedAmmoType.DEFAULTAMMO;
        secondAmmoType = EquippedAmmoType.DEFAULTAMMO;
        currentAmmoType = EquippedAmmoType.DEFAULTAMMO;
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
                if(currentShield == null)
                {
                    currentShield = Instantiate(halfShield, transform.position, Quaternion.identity);
                    currentShield.transform.forward = transform.forward;
                    currentShield.GetComponent<ShieldBehaviour>().IsPCShield(true);
                }
                else
                {
                    currentShield.GetComponent <ShieldBehaviour>().ResetShield();
                }
                break;
            case EquippedAmmoType.FIREPOISON:
                GetBulletObject(BaseBullet.BulletTypes.FIREPOISON);
                break;
            case EquippedAmmoType.POISONFIRE:
                GetBulletObject(BaseBullet.BulletTypes.POISONFIRE);
                break;
            case EquippedAmmoType.FIRESLOW:
                GetBulletObject(BaseBullet.BulletTypes.FIRESLOW);
                break;
            case EquippedAmmoType.POISONSLOW:
                GetBulletObject(BaseBullet.BulletTypes.POISONSLOW);
                break;
            case EquippedAmmoType.SLOWPOISON:
                GetBulletObject(BaseBullet.BulletTypes.SLOWPOISON);
                break;
            case EquippedAmmoType.POISONPOISON:
                GetBulletObject (BaseBullet.BulletTypes.POISONPOISON);
                break;
            case EquippedAmmoType.SLOWFIRE:
                GetBulletObject(BaseBullet.BulletTypes.SLOWFIRE);
                break;
            case EquippedAmmoType.FIREFIRE:
                GetBulletObject(BaseBullet.BulletTypes.FIREFIRE);
                break;
            case EquippedAmmoType.SLOWSLOW:
                GetBulletObject(BaseBullet.BulletTypes.SLOWSLOW);
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
                    firstAmmo.maxValue = ammoCount;
                    secondAmmo.maxValue = ammoCount;
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
    public void ReduceAmmoCount(int damage)
    {
        ammoCount -= damage;
    }
    public EquippedAmmoType GetCurrentAmmoType()
    {
        return currentAmmoType;
    }
    public int GetAmmoCount() 
    {
        return ammoCount;
    }
}
