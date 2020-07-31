using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Weapon
{
    public float time = 2f;
    public float explosionForce = 5000f;
    public float explosionRadius = 10f;
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

        AkSoundEngine.PostEvent("player_grenade_explosion", gameObject);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius,layer);
        foreach (Collider hit in colliders)
        {
            Rigidbody hrb = hit.GetComponent<Rigidbody>();

            if (hrb != null)
            {
                if(hit.gameObject.name == "BossTomato" || hit.gameObject.name == "BossSalchicha")
                {
                    Debug.Log("Los Bosses no son afectados por las fuerzas de las granadas");
                }
                else
                {
                    hrb.AddExplosionForce(explosionForce, explosionPos, explosionRadius);
                }
            }    
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
