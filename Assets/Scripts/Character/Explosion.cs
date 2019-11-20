using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float time = 2f;
    public float explosionForce = 5000f;
    public float explosionRadius = 10f;
    public int minExplosionDamage = 20;
    public int maxExplosionDamage = 50;
    public LayerMask layer;
    public GameObject explosion;
    public GameObject heatWave;

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
            Enemy enemy = hit.GetComponent<Enemy>();

            int randDmg = (int)Random.Range(minExplosionDamage, maxExplosionDamage);
            enemy.TakeDamage(randDmg);

            if (hrb != null)
            {
                hrb.AddExplosionForce(explosionForce, explosionPos, explosionRadius);
            }    
        }
        CameraShakeController.Get().ActiveScreenShake();
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
