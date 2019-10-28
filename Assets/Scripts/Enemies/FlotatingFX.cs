using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//By Ivan Ignacio Castellano - Enjoy :)

public class FlotatingFX : MonoBehaviour
{
    [Header("General Settings")]
    public bool active = false;
    public bool y = true;
    public bool x = true;
    public bool z = true;

    [Header("Vertical Settings")]
    public float secondsBetweenChange= 2f;
    public float speed = 0.05f;

    [Header("Horizontal Settings")]
    public float hDegreesPerSecond = 15.0f;
    public float hAmplitude = 0.5f;
    public float hFrequency = 1f;

    [Header("Z Settings")]
    public float zDegreesPerSecond = 15f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    bool floatUp = true;
    float timer = 0f;

    private void Start()
    {
        posOffset = transform.position;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (active)
        {
            if (x)
            {
                // Spin object around Y-Axis
                transform.Rotate(new Vector3(0f, Time.deltaTime * hDegreesPerSecond, 0f), Space.Self);
            }

            if (z)
            {
                // Spin object around Z-Axis
                transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * zDegreesPerSecond), Space.Self);
            }

            if (y)
            {
                if (floatUp)
                {
                    floatingUp();
                    if (timer >= secondsBetweenChange)
                    {
                        floatUp = false;
                        timer = 0f;
                    }
                }
                else
                {
                    floatingDown();
                    if (timer >= secondsBetweenChange)
                    {
                        floatUp = true;
                        timer = 0f;
                    }
                }
            }
        }       
    }

    void floatingUp()
    {
        tempPos = transform.position;
        tempPos.y += speed * Time.deltaTime;
        transform.position = tempPos;
    }

    void floatingDown()
    {
        tempPos = transform.position;
        tempPos.y -= speed * Time.deltaTime;
        transform.position = tempPos;
    }
}
