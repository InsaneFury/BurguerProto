using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    Vector3 cameraForward;
    Vector3 cameraRight;

    [Header("Weapons Settings")]
    public GameObject machineGun;
    public bool machineGunIsActive = false;
    [HideInInspector]
    public Animator animMachineGun;
    public Gun granade;
    public GameObject sword;
    public float meleeMaxCooldown = 5f;
    float comboMeleeTimer = 0f;
    bool swordIsActive = false;
    public int currentActiveWeapon;

    [Header("Particles VFX")]
    public ParticleSystem healthVFX;
    public ParticleSystem muzzleFlash;

    [Header("UI VFX")]
    public GameObject getDmgVFX;

    Rigidbody rb;
    Vector3 pointToLook = Vector3.zero;
    [HideInInspector]
    public Vector3 forward;
    [HideInInspector]
    public bool isAlive = true;
    public bool isDashing = false;
    public bool isMeleeing = false;
    public bool canPlay = false;

    float originalLife = 0;
    int comboCounter = 0;

    //CHEAT ZARLANGA
    bool notCheating = false;

    public static event Action<Player> OnChangeWeapon;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalLife = life;
        animMachineGun = machineGun.GetComponent<Animator>();
        comboMeleeTimer = meleeMaxCooldown;
        //Audio
        AkSoundEngine.SetState("Vivo_o_muerto", "Vivo");
        currentActiveWeapon = 1;
    }

    void Update()
    {
        float rvertical = Input.GetAxis("Mouse Y");
        float rhorizontal = Input.GetAxis("Mouse X");
        Debug.Log("rvertical: " + rvertical + "  /  " + "rhorizontal:" + rhorizontal);

        if (isAlive && canPlay)
        {
            //CHEAT ZARLANGA
            if (Input.GetKeyDown(KeyCode.Z))
            {
                notCheating = !notCheating;
                if(notCheating)
                    Debug.Log("GOD MODE ZARLANGA ENABLE");
                else
                    Debug.Log("GOD MODE ZARLANGA DISABLE");
            }

            if (isMeleeing)
            {
                comboMeleeTimer -= Time.deltaTime;
            }
            if (swordIsActive)
            {
                SwordAttack();
            }
            
            WeaponChanger();
            Move();
            RotateToMouse();
            if (Input.GetButtonDown("Jump"))
            {
                Dash();
            }
            Heal();

            AkSoundEngine.SetRTPCValue("vida_hamburguesa", life);
            AkSoundEngine.SetRTPCValue("mana_hamburguesa", soulsCollected);
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

        if (machineGunIsActive && (movement.x > 0 || movement.z > 0))
        {
            animMachineGun.SetBool("run", true);
        }
        if(machineGunIsActive && (movement.x <= 0 || movement.z <= 0))
        {
            animMachineGun.SetBool("run", false);
        }
        if(movement.x == 0 && movement.z == 0)
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
        if (!isMeleeing)
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
        if (!notCheating)
        {
            if (isAlive)
            {
                if (life > 0)
                    life -= dmg;

                //Audio
                AkSoundEngine.PostEvent("Voz_hamburguesa", gameObject);
                AkSoundEngine.PostEvent("Damage_hamburguesa", gameObject);
                StartCoroutine(GettingDmgVFX());
            }
            else
            {
                getDmgVFX.SetActive(true);
            }

            Death();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Soul"))
        {
            //Audio
            AkSoundEngine.PostEvent("Fantasmas_enemigos", gameObject);
            if(soulsCollected < maxSoulsCollected)
            {
                soulsCollected += (int)UnityEngine.Random.Range(soulGainRange.x, soulGainRange.y);
            }
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Granade"))
        {
            Gun.Get().granades++;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Ammo"))
        {
            MachineGun.Get().bullets+=25;
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

        //Audio
        AkSoundEngine.PostEvent("Dash", gameObject);

        if (playerMove == Vector3.zero)
        {
            rb.velocity = cameraForward * dashSpeed;
        }
        else
        {
            rb.velocity = playerMove * dashSpeed;
        }
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

    IEnumerator GettingDmgVFX()
    {
        getDmgVFX.SetActive(true);
        yield return new WaitForSeconds(1);
        getDmgVFX.SetActive(false);
    }

    void Heal()
    {
        bool canHeal = (soulsCollected >= healCost) && (life < originalLife);

        if (Input.GetKeyDown(KeyCode.E) && canHeal)
        {
            life += healAmount;
            soulsCollected -= healCost;

            //Audio
            AkSoundEngine.PostEvent("Curar_vida", gameObject);

            healthVFX.Play();
        }
    }

    void Death()
    {
        if(life <= 0)
        {
            isAlive = false;

            //Audio
            AkSoundEngine.PostEvent("Muerte_hamburguesa", gameObject);
            AkSoundEngine.SetState("Vivo_o_muerto", "Muerto");

            animBottom.SetBool("death", true);
            animTop.SetBool("death", true);
        }
    }

    void SwordAttack()
    {
        if (comboMeleeTimer <= 0)
        {
            comboCounter = 0;
            isMeleeing = false;

            sword.GetComponent<BoxCollider>().enabled = false;
            animBottom.SetBool("exitCombo",true);
            animTop.SetBool("exitCombo",true);

            animTop.ResetTrigger("swordOne");
            animTop.SetBool("swordTwo",false);
            animTop.SetBool("swordThree", false);
            comboMeleeTimer = meleeMaxCooldown;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine("ResetAnimationsTriggers");
            comboCounter++;
            

            if (comboCounter == 1)
            {
                animTop.SetTrigger("swordOne");
                comboMeleeTimer = meleeMaxCooldown;
                isMeleeing = true;
                sword.GetComponent<BoxCollider>().enabled = true;
            }
            if ((comboCounter == 2) && (comboMeleeTimer > 0))
            {
                animTop.SetBool("swordTwo",true);
                comboMeleeTimer = meleeMaxCooldown;
                sword.GetComponent<BoxCollider>().enabled = true;
            }
            if ((comboCounter == 3) && (comboMeleeTimer > 0))
            {
                animTop.SetBool("swordThree", true);
                comboMeleeTimer = meleeMaxCooldown;
                sword.GetComponent<BoxCollider>().enabled = true;
            } 
        }
    }

    //Refactorizar WeaponChanger
    void WeaponChanger()
    {
        if (Input.GetKey(KeyCode.Alpha1) && !machineGunIsActive)
        {
            currentActiveWeapon = 0;
            //Audio
            AkSoundEngine.PostEvent("Mch_Gun_cambio", gameObject);

            machineGunIsActive = true;
            machineGun.SetActive(machineGunIsActive);
            granade.enabled = false;
            sword.SetActive(false);
            swordIsActive = false;
            animBottom.SetBool("exitCombo", true);
            animTop.SetBool("exitCombo", true);
            animBottom.SetTrigger("resetMove");
            animTop.SetTrigger("resetMove");

            if (OnChangeWeapon != null)
                OnChangeWeapon(this);
        }
        if (Input.GetKey(KeyCode.Alpha2) && !granade.enabled)
        {
            currentActiveWeapon = 1;
            //Audio
            AkSoundEngine.PostEvent("Granada_cambio", gameObject);

            machineGunIsActive = false;
            machineGun.SetActive(machineGunIsActive);
            granade.enabled = true;
            sword.SetActive(false);
            swordIsActive = false;
            animBottom.SetBool("exitCombo", true);
            animTop.SetBool("exitCombo", true);
            animBottom.SetTrigger("resetMove");
            animTop.SetTrigger("resetMove");

            if (OnChangeWeapon != null)
                OnChangeWeapon(this);
        }
        if (Input.GetKey(KeyCode.Alpha3) && !swordIsActive)
        {
            currentActiveWeapon = 2;
            //Audio
            AkSoundEngine.PostEvent("Espada_cambio", gameObject);

            machineGunIsActive = false;
            machineGun.SetActive(machineGunIsActive);
            granade.enabled = false;
            sword.SetActive(true);
            swordIsActive = true;
            animBottom.SetTrigger("resetMove");
            animTop.SetTrigger("resetMove");

            if (OnChangeWeapon != null)
                OnChangeWeapon(this);
        }

        

        if (machineGunIsActive)
        {
            animTop.SetBool("machineGun", machineGunIsActive);
        }
        else
        {
            animTop.SetBool("machineGun", machineGunIsActive);
        }
    }

    IEnumerator ResetAnimationsTriggers()
    {

        yield return new WaitForSecondsRealtime(1);
        animBottom.SetBool("exitCombo", false);
        animTop.SetBool("exitCombo", false);
    }

    public void ResetStats()
    {
        //Audio
        AkSoundEngine.SetState("Vivo_o_muerto", "Vivo");

        animBottom.SetBool("death", false);
        animTop.SetBool("death", false);
        life = originalLife;
        isAlive = true;
        getDmgVFX.SetActive(false);
        soulsCollected = 100;

        //Back to default weapon
        machineGunIsActive = false;
        machineGun.SetActive(machineGunIsActive);
        granade.enabled = true;
        sword.SetActive(false);
        swordIsActive = false;
    }

    
}
