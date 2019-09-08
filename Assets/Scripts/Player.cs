using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonobehaviourSingleton<Player>
{
    public Transform crosshair;
    public Transform vision;

    public float speed = 0.5f;
    public float rotationSpeed = 0.5f;

    public float rayLenght = 100;
    public float life = 100;

    public Animator animTop;
    public Animator animBottom;

    public GameObject Top;

    Rigidbody rb;
    Vector3 pointToLook = Vector3.zero;
    [HideInInspector]
    public Vector3 forward;

    public Camera cam;
    Vector3 cameraForward;
    Vector3 cameraRight;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        RotateToMouse();
    }

    void Move()
    {
        Vector3 playerMove;

        float vertical = Input.GetAxis("Vertical") * speed;
        float horizontal = Input.GetAxis("Horizontal") * rotationSpeed;

        vertical *= Time.fixedDeltaTime;
        horizontal *= Time.fixedDeltaTime;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement =  Vector3.ClampMagnitude(movement, 1);

        CameraDirection();

        playerMove = movement.x * cameraRight + movement.z * cameraForward;

        Quaternion turn = transform.rotation;
        turn.SetLookRotation(transform.position + playerMove * rotationSpeed);
        transform.rotation = new Quaternion(0f,turn.y,0f,turn.w);      

        animBottom.SetFloat("horizontal", horizontal);
        animBottom.SetFloat("vertical", vertical);
        animTop.SetFloat("horizontal", horizontal);
        animTop.SetFloat("vertical", vertical);
       
        rb.AddForce(playerMove * speed * Time.fixedDeltaTime,ForceMode.VelocityChange);

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
        Top.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    void CameraDirection()
    {
        cameraForward = cam.transform.forward;
        cameraRight = cam.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        UIManager.Get().RefreshHealthbar();
    }
}
