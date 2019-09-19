using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonobehaviourSingleton<Player>
{
    [Header("Mouse Vision")]
    public Transform crosshair;
    public Transform vision;
    public float rayLenght = 100;

    [Header("Movement Speed")]
    public float speed = 0.5f;
    public float rotationSpeed = 0.5f;
    Vector3 playerMove;

    [Header("Dash")]
    public float dashSpeed = 100f;
    public float dashTime = 1f;
    public GameObject trail;
    public Material topMat;
    public Material bottomMat;

    [Header("Player Settings")]
    public float life = 100;

    [HideInInspector]
    public int soulsCollected = 0;

    [Header("Animator Settings")]
    public Animator animTop;
    public Animator animBottom;
    public GameObject Top;

    Rigidbody rb;
    Vector3 pointToLook = Vector3.zero;
    [HideInInspector]
    public Vector3 forward;

    [Header("Camera Settings")]
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
        if (Input.GetButtonDown("Jump"))
        {
            Dash();
        }
    }

    void Move()
    {
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

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Soul"))
        {
            UIManager.Get().RefreshSouls();
            Destroy(other.gameObject);
        }
    } 

    void Dash()
    {
        StartCoroutine(ActiveDashTrail());
        rb.velocity = playerMove * dashSpeed;
    }

    IEnumerator ActiveDashTrail()
    {
        trail.SetActive(true);
        topMat.EnableKeyword("_EMISSION");
        bottomMat.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(dashTime);
        trail.SetActive(false);
        topMat.DisableKeyword("_EMISSION");
        bottomMat.DisableKeyword("_EMISSION");
    }
}
