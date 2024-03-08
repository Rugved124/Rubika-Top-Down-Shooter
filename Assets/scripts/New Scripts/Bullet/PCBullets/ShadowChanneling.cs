using System.Collections;
using UnityEngine;

public class ShadowChanneling : BaseBullet
{

    private float shadowTrackingTime;

    bool isPlayerHit;

    public GameObject fallingObject;

    bool some;
    public override void Start()
    {
        base.Start();
        bulletType = BulletTypes.SLOWSLOW;
        shadowTrackingTime = bulletLifeTime - 1f;
        transform.position = InputManager.instance.GetMousePosition();
        this.transform.localScale = new Vector3(transform.localScale.x - shadowTrackingTime, transform.localScale.y, transform.localScale.z - shadowTrackingTime);
    }

    public override void Update()
    {
        base.Update();
        shadowTrackingTime -= Time.deltaTime;
        if (shadowTrackingTime <= 0f)
        {
            StartCoroutine(DieCoRoutine());

            if (!some)
            {
                Invoke("Die", 1f);
                some = true;
            }



        }
        else
        {
            if (!pc.isDead)
            {
                this.transform.localScale += new Vector3(Time.deltaTime, 0, Time.deltaTime);
            }
            else
            {
                Die();
            }

        }
    }

    IEnumerator DieCoRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        if (!isPlayerHit)
        {
            isPlayerHit = true;
            Vector3 debryPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            GameObject _fallingObject = Instantiate(fallingObject, debryPos, Quaternion.identity);
            _fallingObject.GetComponent<Debries>().SetIfPC(true);
            _fallingObject.GetComponent<Debries>().GetDamage(bulletDamage);
        }
        //Collider collider = GetComponent<Collider>();
        //collider.enabled = true;
    }
}
