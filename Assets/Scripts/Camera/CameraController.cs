using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    Transform target;
    [Range(0, 1)] public float lerpValue;
    public float sensibility;
         
    void Start()
    {
        target = GameObject.Find("Player").transform;
        FollowTarget();
        RotateCamera();
    }

    void LateUpdate()
    {
        FollowTarget();
        if (Input.GetMouseButton(1))
        {
            RotateCamera();
        }
    }

    void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpValue);
    }

    void RotateCamera()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensibility, Vector3.up) * offset;
        transform.LookAt(target);
    }
}
