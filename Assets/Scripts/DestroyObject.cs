using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float time = 2f;
    void Start()
    {
        Invoke("DestroyInTime", time);
    }

    void DestroyInTime()
    {
        Destroy(gameObject);
    }
}
