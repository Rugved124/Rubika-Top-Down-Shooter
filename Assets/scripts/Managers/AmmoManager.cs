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

    public enum EquippedAmmoType
    {
        DEFAULTAMMO,
        FIRE,
        POISON,
        SLOW,
        FIREPOISON,
        FIRESLOW,
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
    }
    private void Update()
    {
        if(InputManager.instance.IsMousePressed()) 
        {
            if(bulletToSpawn != null)
            {
                Instantiate(bulletToSpawn, pc.GetPCShootPos(), Quaternion.identity);
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

}
