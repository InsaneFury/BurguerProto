using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    Player player;
    public LayerMask layer;
    public float pullRadius = 10;
    public float pullForce = 1;

    private void Start()
    {
        player = Player.Get();
    }

    private void FixedUpdate()
    {
        CollectItemUsingPhysics();
    }

    void CollectItemUsingPhysics()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius,layer))
        {
            // calculate direction from target to me
            Vector3 forceDirection = transform.position - collider.transform.position;

            // apply force on target towards me
            collider.GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, pullRadius);
    }


}
