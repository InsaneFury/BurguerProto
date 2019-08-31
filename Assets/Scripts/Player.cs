using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonobehaviourSingleton<Player>
{
    public Transform crosshair;
    public Transform vision;

    public float speed =  5;
    public float rayLenght = 100;
    public float life = 100;

    public Animator anim;

    Rigidbody rb;
    Vector3 pointToLook = Vector3.zero;
    [HideInInspector]
    public Vector3 forward;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        anim.SetBool("run", false);
        Move();
        RotateToMouse();
    }


    void Move()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("run", true);
            direction += Vector3.left;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("run", true);
            direction += Vector3.right;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("run", true);
            direction += Vector3.forward;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("run", true);
            direction += Vector3.back;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 315, 0);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 135, 0);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 225, 0);
        }

        Vector3 temp = Vector3.ClampMagnitude(direction, 1f) * speed * Time.fixedDeltaTime;

        rb.AddForce(temp, ForceMode.VelocityChange);
    }

    void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up,Vector3.zero);

        if (groundPlane.Raycast(ray,out rayLenght))
        {
            pointToLook = ray.GetPoint(rayLenght);
            Debug.DrawLine(ray.origin, pointToLook, Color.blue);
        }

        forward = new Vector3(pointToLook.x,transform.position.y,pointToLook.z) - transform.position;

        if(crosshair)
        crosshair.position = pointToLook;

        vision.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(collision.collider.gameObject.GetComponent<Enemy>().damage);
            UIManager.Get().RefreshHealthbar();
        }
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
    }
}
