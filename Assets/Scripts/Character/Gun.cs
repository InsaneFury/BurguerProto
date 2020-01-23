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

    float timeToFire = 0f;
    float originalSize = 0;

    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        gManager = GameManager.Get();
        bulletSize = bullet.transform.localScale.x;
        player = Player.Get();
        originalSize = bulletSize;
    }

    public void Shoot()
    {
        if ((player.isAlive && gManager.gameStarted) && !gManager.pause)
        {
            if ((granades > 0))
            {
                granades--;
                player.animTop.SetTrigger("attack");
            }
        }
    }

    public void InstantiateGranade()
    {
        Vector3 spawnPos = Player.Get().transform.position;

        GameObject b = Instantiate(bullet, spawnPos + player.forward.normalized * dropDistance, player.transform.rotation);

        b.GetComponent<Rigidbody>().AddForce(player.forward * shootPower * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
