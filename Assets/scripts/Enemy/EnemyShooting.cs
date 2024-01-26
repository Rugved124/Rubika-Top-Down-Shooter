using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    [SerializeField] private GameObject bullet; // a link to the bullet gameobject
    [SerializeField] private Transform attackPoint; // the location where the bullets will spawn
    [SerializeField] private float shootForce, upwardForce; //force applied on the bullets

    [SerializeField] private float timeBetweenShooting, timeBetweenShots, spread, reloadTime; // bullet stats

    [SerializeField] private int magazineSize, bulletsperTap; // gun stats 

    [SerializeField] private bool allowButtonHold; // to check if the gun is automatic fire on hold or tapfire
    [SerializeField] private Transform bullDir;
    private int bulletsLeft, bulletsShot;

    private bool readyToShoot, shooting, reloading;

    EnemyMovement enemy;
    PCController pcLocation;
    public bool allowInvoke = true;
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        enemy = GetComponent<EnemyMovement>();
        pcLocation = FindObjectOfType<PCController>();
    }


    void Update()
    {
        if (enemy.canShoot)
        {
            transform.LookAt(pcLocation.transform.position);
            shooting = true;
            ReloadAndShoot();
        }

        
    }

    private void ReloadAndShoot()
    { 
        //reloading
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //set bullets shot to 0
            bulletsShot = 0;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        bulletsLeft--;
        bulletsShot++;

        Vector3 directionWithoutSpread = pcLocation.transform.position - transform.position;
        directionWithoutSpread.y = 0;
        //calculate the spread
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        Vector3 bulletDirection = bullDir.position - transform.position;
        //calculate the direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY, 0);

        //Instantiate bullet projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.transform.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;  
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
