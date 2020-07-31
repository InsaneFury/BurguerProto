using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Granade : MonobehaviourSingleton<Granade>
{
    public GameObject bullet;
    public float bulletSize = 0.1f;
    public float maxBulletSize = 0.9f;
    public float speedOfIncrease = 0.1f;
    public float dropDistance = 1;
    public float shootPower = 10f;
    public float fireRate = 2f;
    public int granadeCount = 2;

    private Player player;
    private GameManager gameManager;
    private Mouse mouse;
    private Gamepad gamepad;

    public override void Awake()
    {
        base.Awake(); 
    }

    void Start()
    {
        mouse = InputSystem.GetDevice<Mouse>();
        gamepad = InputSystem.GetDevice<Gamepad>();
        player = Player.Get();
        gameManager = GameManager.Get();
        bulletSize = bullet.transform.localScale.x;
    }

    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        Gamepad gp = Gamepad.current;
        bool joystickActive = false;
        if((gp != null) && gamepad.rightTrigger.wasPressedThisFrame)
        {
            joystickActive = true;
        }

        if ((player.isAlive && gameManager.gameStarted) && !gameManager.pause)
        {
            if (mouse.leftButton.wasPressedThisFrame || joystickActive && (granadeCount > 0))
            {
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
