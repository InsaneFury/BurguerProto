using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float time = 2f;
    public float explosionForce = 400f;
    public float explosionRadius = 100f;
    public LayerMask layer;
    public GameObject explosion;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Explode", time);
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius,layer);
        foreach (Collider hit in colliders)
        {
            Rigidbody hrb = hit.GetComponent<Rigidbody>();
            Debug.Log(hit.transform.name);

            if (hrb != null)
            {
                hrb.AddExplosionForce(explosionForce, explosionPos, explosionRadius);
            }
               
        }
        Destroy(gameObject);
    }
}
