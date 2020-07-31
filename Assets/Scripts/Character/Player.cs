using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEditor;

public class Player : MonobehaviourSingleton<Player>
{

    [Header("Mouse Vision")]
    public Transform crosshair;
    public Transform vision;
    public float rayLenght = 100;

    [Header("Movement Speed")]
    public float speed = 0.5f;
    public float rotationSpeed = 0.5f;
    private Vector3 playerMove;

    [Header("Dash")]
    public float dashSpeed = 100f;
    public float dashTime = 1f;
    public GameObject trail;
    public Material topMat;
    public Material bottomMat;
    public int dashCost = 2;

    [Header("Player Settings")]
    public float life = 100;
    public Animator face;

    [Header("Souls")]
    public int soulsCollected = 0;
    public Vector2 soulGainRange;
    public int maxSoulsCollected = 100;

    [Header("Heal")]
    public float healAmount = 0.1f;
    public int healCost = 1;

    [Header("Animator Settings")]
    public Animator animTop;
    public Animator animBottom;
    public GameObject Top;

    [Header("Camera Settings")]
    public Camera cam;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    [Header("Weapons Settings")]
    public GameObject machineGun;
    public bool machineGunIsActive = false;
    [HideInInspector]
    public Animator animMachineGun;
    public Granade granade;
    public int currentActiveWeapon;
    public int machineGunBullets;

    [Header("Particles VFX")]
    public ParticleSystem healthVFX;
    public ParticleSystem muzzleFlash;

    [Header("UI VFX")]
    public GameObject getDmgVFX;

    [HideInInspector]
    public Vector3 forward;
    [HideInInspector]
    public bool isAlive = true;
    public bool isDashing = false;
    public bool isMeleeing = false;
    public bool canPlay = false;

    private Rigidbody rb;
    private Vector3 pointToLook = Vector3.zero;
    private float originalLife = 0;

    //CHEAT ZARLANGA
    private bool notCheating = false;

    //New Input System
    public PlayerInputActions inputAction;
    private Vector2 movementInput;
    private Vector2 look;
    private Keyboard kb;
    private float aimAngle;
    private Quaternion aimRotation;
    private float lastPosition = 0;

    public static event Action<Player> OnChangeWeapon;
    public static event Action<Player> OnPlayerDead;

    public override void Awake()
    {
        base.Awake();
        inputAction = new PlayerInputActions();
       
    }

    void Start()
    {

        //All the pctx ctx etc mean ctx = context and the first letter just from heal, dash etc
        //Input System (assign movement input vector2 to movementInput new variable)
        inputAction.PlayerControls.Move.performed += pctx => movementInput = pctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Look.performed += lctx => look = lctx.ReadValue<Vector2>();
        //Input System (its like a delegate assign a function or multiple functions to an input action)
        inputAction.PlayerControls.Heal.performed += hctx => Heal();
        inputAction.PlayerControls.Dash.performed += dctx => Dash();
        inputAction.PlayerControls.WeaponChange.performed += wctx => WeaponChanger();
        rb = GetComponent<Rigidbody>();
        kb = InputSystem.GetDevice<Keyboard>();
        animMachineGun = machineGun.GetComponent<Animator>();
        
        originalLife = life;
        currentActiveWeapon = 1;
        WeaponChanger();
        machineGunBullets = 100;
    }

    void Update()
    {
        //Gamepad detection
        Gamepad gp = Gamepad.current;
            
        if (isAlive && canPlay)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                BecomeInmortal();
            }

            Move();

            if (gp != null) RotateWithJoystick(); 
            else RotateToMouse();
        }
    }
    public void ResetStats()
    {
        animBottom.SetBool("death", false);
        animTop.SetBool("death", false);
        life = originalLife;
        isAlive = true;
        getDmgVFX.SetActive(false);
        soulsCollected = 100;
        Granade.Get().granadeCount = 2;
        //MachineGun.Get().bullets = 100;

        //Back to default weapon
        machineGunIsActive = false;
        machineGun.SetActive(machineGunIsActive);
        granade.enabled = true;
    }
    public void BecomeInmortal()
    {
        notCheating = !notCheating;
        if (notCheating)
            Debug.Log("GOD MODE ZARLANGA ENABLE");
        else
            Debug.Log("GOD MODE ZARLANGA DISABLE");
    }
    public void TakeDamage(float dmg)
    {
        if (notCheating) return;
        if (isAlive)
        {
            if (life > 0)
                life -= dmg;
            AkSoundEngine.PostEvent("player_get_damage", gameObject);
            StartCoroutine(GettingDmgVFX());
        }
        else
        {
            getDmgVFX.SetActive(true);
        }
        Death();
    }
    private void Move()
    {
        float vertical = movementInput.y * speed;
        float horizontal = movementInput.x * rotationSpeed;

        vertical *= Time.fixedDeltaTime;
        horizontal *= Time.fixedDeltaTime;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = Vector3.ClampMagnitude(movement, 1);

        CameraDirection();

        playerMove = movement.x * cameraRight + movement.z * cameraForward;

        Quaternion turn = transform.rotation;
        turn.SetLookRotation(transform.position + playerMove * rotationSpeed);
        transform.rotation = new Quaternion(0f, turn.y, 0f, turn.w);

        if (isDashing)
            Top.transform.rotation = transform.rotation;

        if (machineGunIsActive && (movement.x > 0 || movement.z > 0))
        {
            animMachineGun.SetBool("run", true);
        }
        if (machineGunIsActive && (movement.x <= 0 || movement.z <= 0))
        {
            animMachineGun.SetBool("run", false);
        }
        if (movement.x == 0 && movement.z == 0)
        {
            face.SetBool("run", false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            face.SetBool("run", true);
        }

        animBottom.SetFloat("horizontal", horizontal);
        animBottom.SetFloat("vertical", vertical);
        animTop.SetFloat("horizontal", horizontal);
        animTop.SetFloat("vertical", vertical);

        rb.AddForce(playerMove * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

    }
    private void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(look);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out rayLenght))
        {
            pointToLook = ray.GetPoint(rayLenght);
            Debug.DrawLine(ray.origin, pointToLook, Color.blue);
        }

        forward = new Vector3(pointToLook.x, transform.position.y, pointToLook.z) - transform.position;

        if (crosshair)
            crosshair.position = pointToLook;

        vision.rotation = Quaternion.LookRotation(forward, Vector3.up);
        if (!isDashing)
        {
            Top.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
        if (!isMeleeing)
        {
            Top.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
    private void RotateWithJoystick()
    {

        if (look.x > 0.03 || look.x < 0 || look.y > 0.03 || look.y < 0)
        {
            aimAngle = Mathf.Atan2(look.x, look.y) * Mathf.Rad2Deg;
            lastPosition = aimAngle;
        }
        else
        {
            aimAngle = lastPosition;
        }


        aimRotation = Quaternion.AngleAxis(aimAngle, Vector3.up);
        vision.rotation = Quaternion.Slerp(vision.transform.rotation, aimRotation, rotationSpeed * Time.deltaTime);

        if (!isDashing || !isMeleeing)
        {
            Top.transform.rotation = Quaternion.Slerp(Top.transform.rotation, aimRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void CameraDirection()
    {
        cameraForward = cam.transform.forward;
        cameraRight = cam.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
    }
    private void Dash()
    {
        if (soulsCollected < dashCost)
            return;
        soulsCollected -= dashCost;
        StartCoroutine(ActiveDashTrail());
        AkSoundEngine.PostEvent("player_dash", gameObject);
        rb.velocity = (playerMove == Vector3.zero ? cameraForward : playerMove) * dashSpeed;
    }
    private IEnumerator ActiveDashTrail()
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
    private IEnumerator GettingDmgVFX()
    {
        getDmgVFX.SetActive(true);
        yield return new WaitForSeconds(1);
        getDmgVFX.SetActive(false);
    }
    private void Heal()
    {
        bool canHeal = (soulsCollected >= healCost);
        if (!canHeal) return;
        life += healAmount;
        if (life > originalLife)
            life = originalLife;

        soulsCollected -= healCost;
        AkSoundEngine.PostEvent("player_heal", gameObject);
        healthVFX.Play();
    }
    private void Death()
    {
        if (life > 0) return;
        isAlive = false;
        AkSoundEngine.PostEvent("player_dead", gameObject);
        animBottom.SetBool("death", true);
        animTop.SetBool("death", true);
        OnPlayerDead?.Invoke(this);
    }
    //Refactorizar WeaponChanger
    private void WeaponChanger()
    {
        currentActiveWeapon++;
        if (currentActiveWeapon > 2)
           currentActiveWeapon = 0;

        if(currentActiveWeapon == 0)
        {
            granade.enabled = false;
            machineGun.SetActive(true);
            machineGunIsActive = true;
            animTop.SetBool("machineGun", true);
            AkSoundEngine.PostEvent("player_change_machinegun", gameObject);
        }
        else if(currentActiveWeapon == 1)
        {
            granade.enabled = true;
            machineGun.SetActive(false);
            machineGunIsActive = false;
            animTop.SetBool("machineGun", false);
            AkSoundEngine.PostEvent("player_change_grenade", gameObject);
        }
       

        if (OnChangeWeapon != null)
            OnChangeWeapon(this);
        animBottom.SetTrigger("resetMove");
        animTop.SetTrigger("resetMove");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Soul"))
        {
            AkSoundEngine.PostEvent("player_pick_ghost", gameObject);
            if (soulsCollected < maxSoulsCollected)
            {
                soulsCollected += (int)UnityEngine.Random.Range(soulGainRange.x, soulGainRange.y);
            }
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Granade"))
        {
            Granade.Get().granadeCount++;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Ammo"))
        {
            MachineGun.Get().bullets += 25;
            Destroy(other.gameObject);
        }
    }
    //INPUTS FUNCTIONS
    private void OnEnable() { inputAction.Enable(); }
    private void OnDisable() => inputAction.Disable();

}
