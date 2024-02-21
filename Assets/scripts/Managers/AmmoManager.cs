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

    bool canShoot;

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
        POISONSLOW

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
            if(bulletToSpawn != null && canShoot)
            {
                canShoot = false;
                StartCoroutine(bulletFireRate());
                Instantiate(bulletToSpawn, pc.GetPCShoot().position, Quaternion.identity);
            }
            
        }
    }
    public void ChangeAmmoType()
    {
        switch (currentAmmoType)
        {
            case EquippedAmmoType.DEFAULTAMMO:

                GetBulletObject(BaseBullet.BulletTypes.DEFAULTAMMO);

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
