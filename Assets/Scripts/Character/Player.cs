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
    public int dashCost = 2;

    [Header("Player Settings")]
    public float life = 100;

    [Header("Souls")]
    public int soulsCollected = 0;

    [Header("Heal")]
    public float healAmount = 0.1f;
    public int healCost = 1;


    [Header("Animator Settings")]
    public Animator animTop;
    public Animator animBottom;
    public GameObject Top;

    [Header("Camera Settings")]
    public Camera cam;
    Vector3 cameraForward;
    Vector3 cameraRight;

    Rigidbody rb;
    Vector3 pointToLook = Vector3.zero;
    [HideInInspector]
    public Vector3 forward;
    public bool isAlive = true;
    bool isDashing = false;
    float originalLife = 0;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalLife = life;
    }

    void Update()
    {
        if(isAlive)
        {
            Move();
            RotateToMouse();
            if (Input.GetButtonDown("Jump"))
            {
                Dash();
            }
            Heal();
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

        if(isDashing)
        Top.transform.rotation = transform.rotation;

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
        if (!isDashing)
        {
            Top.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }    
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
        Death();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Soul"))
        {
            UIManager.Get().RefreshSouls();
            soulsCollected++;
            Destroy(other.gameObject);
        }
    } 

    void Dash()
    {
        if(soulsCollected < dashCost)
        {
            return;
        }
        
        soulsCollected -= dashCost;
       
        StartCoroutine(ActiveDashTrail());
        if (playerMove == Vector3.zero)
        {
            rb.velocity = cameraForward * dashSpeed;
        }
        else
        {
            rb.velocity = playerMove * dashSpeed;
        }
        UIManager.Get().RefreshSouls();
    }

    IEnumerator ActiveDashTrail()
    {
        trail.SetActive(true);
        animBottom.SetTrigger("dash");
        animTop.SetTrigger("dash");
        topMat.EnableKeyword("_EMISSION");
        bottomMat.EnableKeyword("_EMISSION");
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        trail.SetActive(false);
        animBottom.ResetTrigger("dash");
        animTop.ResetTrigger("dash");
        topMat.DisableKeyword("_EMISSION");
        bottomMat.DisableKeyword("_EMISSION");
        isDashing = false;
    }

    void Heal()
    {
        bool canHeal = (soulsCollected >= healCost) && (life < originalLife);

        if (Input.GetKey(KeyCode.E) && canHeal)
        {
            life += healAmount;
            soulsCollected -= healCost;
            UIManager.Get().RefreshSouls();
            UIManager.Get().RefreshHealthbar();
        }
    }

    void Death()
    {
        if(life <= 0)
        {
            isAlive = false;
            animBottom.SetBool("death", true);
            animTop.SetBool("death", true);
        }
    }
}
