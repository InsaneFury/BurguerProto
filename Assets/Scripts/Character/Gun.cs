using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonobehaviourSingleton<Gun>
{
    public GameObject bullet;
    public float bulletSize = 0.1f;
    public float maxBulletSize = 0.9f;
    public float speedOfIncrease = 0.1f;
    public float dropDistance = 1;
    public float shootPower = 10f;
    public float fireRate = 2f;
    public int granades = 2;

    Player player;
    GameManager gManager;

    Mouse mouse;
    Gamepad gd;

    float timeToFire = 0f;
    float originalSize = 0;

    public override void Awake()
    {
        base.Awake();
        mouse = InputSystem.GetDevice<Mouse>();
        gd = InputSystem.GetDevice<Gamepad>();
    }

    void Start()
    {
        gManager = GameManager.Get();
        bulletSize = bullet.transform.localScale.x;
        player = Player.Get();
        originalSize = bulletSize;
    }

    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {

        Gamepad gp = Gamepad.current;
        bool joystickActive = false;
        if((gp != null) && gd.rightTrigger.wasPressedThisFrame)
        {
            joystickActive = true;
        }

        if ((player.isAlive && gManager.gameStarted) && !gManager.pause)
        {
            if (mouse.leftButton.wasPressedThisFrame || joystickActive && (granades > 0))
            {
                granades--;
                player.animTop.SetTrigger("attack");
            }
        }
    }

    public void InstantiateGranade()
    {
        Vector3 spawnPos = Player.Get().transform.position;

        GameObject b = Instantiate(bullet, spawnPos + player.Top.transform.forward * dropDistance, player.transform.rotation);

        b.GetComponent<Rigidbody>().AddForce(player.Top.transform.forward * shootPower * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
