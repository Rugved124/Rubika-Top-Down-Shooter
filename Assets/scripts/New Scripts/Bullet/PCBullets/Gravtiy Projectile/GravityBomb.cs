using UnityEngine;

public class GravityBomb : MonoBehaviour
{
    [SerializeField]
    GameObject explosion;

    [SerializeField]
    float maxTime;

    [SerializeField]
    float gravityRange;

    [SerializeField]
    float gravityMultiplier;

    [SerializeField]
    LayerMask enemies;
    void Update()
    {
        maxTime -= Time.deltaTime;
        if(Physics.OverlapSphere(transform.position, gravityRange, enemies).Length > 0)
        {
            foreach(Collider c in Physics.OverlapSphere(transform.position, gravityRange, enemies))
            {
                c.gameObject.GetComponent<Rigidbody>().AddForce((transform.position - c.transform.position).normalized * gravityMultiplier, ForceMode.Force);
            }
        }

        if(maxTime <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Die();
        }
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}
