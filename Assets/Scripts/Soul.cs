using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float speed = 1;
    public float seconds = 2;
    private bool follow = false;

    void Start()
    {
        Invoke("Follow", seconds);
    }

    void Update()
    {
        if (follow)
            GoToPlayer();
    }
    
    void Follow()
    {
        follow = true;
    }

    void GoToPlayer()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, Player.Get().transform.position, step);
    }
}
