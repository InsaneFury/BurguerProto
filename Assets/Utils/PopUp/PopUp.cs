using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 offset;
    public Vector3 randomVariation;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
        transform.localPosition += offset;
        transform.localPosition += new Vector3(
            Random.Range(-randomVariation.x, randomVariation.x), 
            Random.Range(-randomVariation.y, randomVariation.y), 
            Random.Range(-randomVariation.z, randomVariation.z));
    }

}
