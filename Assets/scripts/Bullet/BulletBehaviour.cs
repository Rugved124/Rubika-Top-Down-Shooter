using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    [SerializeField] private GameObject bullet; // a link to the bullet gameobject
    [SerializeField] private Camera cam; // link to the cam
    [SerializeField] private Transform attackPoint; // the location where the bullets will spawn
    [SerializeField] private InputManager mousePos;
    [SerializeField] private float shootForce, upwardForce; //force applied on the bullets

    [SerializeField] private float timeBetweenShooting, timeBetweenShots, spread; // bullet stats

    [SerializeField] private int magazineSize, bulletsperTap; // gun stats 

    [SerializeField] private bool allowButtonHold; // to check if the gun is automatic fire on hold or tapfire
    [SerializeField] private Transform bullDir;
    private int bulletsLeft, bulletsShot;

    private bool readyToShoot, shooting;


    public bool allowInvoke = true;
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }


    void Update()
    {
        MyInput();
        //Debug.DrawLine(transform.position, mousePos.GetMousePosition());
    }

    private void MyInput()
    {
        // check if you are allowed to hold button down
        if (allowButtonHold) shooting = Input.GetMouseButton(0);
        else shooting = Input.GetMouseButtonDown(0);
        //Shooting
        if (readyToShoot && shooting  && bulletsLeft > 0)
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

        Vector3 directionWithoutSpread = mousePos.GetMousePosition() - transform.position;
        directionWithoutSpread.y = 0;
        //calculate the spread
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        Vector3 bulletDirection = bullDir.position - transform.position;
        //calculate the direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY , 0);

        //Instantiate bullet projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.transform.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        if (mousePos.GetMousePosition().z - transform.position.z < 1 && mousePos.GetMousePosition().z - transform.position.z > -1 && mousePos.GetMousePosition().x - transform.position.x < 1 && mousePos.GetMousePosition().x - transform.position.x > -1)
        {
            currentBullet.GetComponent<Rigidbody>().AddForce(bulletDirection.normalized * shootForce, ForceMode.Impulse);
        }
        else
        {
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        }
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
}
