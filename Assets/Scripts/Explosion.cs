using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float time = 2f;
    public float explosionForce = 1000000f;
    public float explosionRadius = 1000000f;
    public LayerMask layer;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Explode", time);
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius,layer);
        foreach (Collider hit in colliders)
        {
            Rigidbody hrb = hit.GetComponent<Rigidbody>();
            Debug.Log(hit.transform.name);

            if (hrb != null)
                hrb.AddExplosionForce(explosionForce, explosionPos, explosionRadius);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
