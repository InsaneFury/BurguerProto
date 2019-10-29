using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonobehaviourSingleton<Gun>
{
    public GameObject bullet;
    public float bulletSize = 0.1f;
    public float maxBulletSize = 0.9f;
    public float speedOfIncrease = 0.1f;
    public float dropDistance = 1;
    public float shootPower = 10f;
    public float fireRate = 2f;

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

    private void Update()
    {
        if (player.isAlive && gManager.gameStarted)
        {
            if ((bulletSize < maxBulletSize) && Input.GetButton("Fire1") && (Time.time >= timeToFire))
            {
                IncreaseBulletSize();
            }
            if (Input.GetMouseButtonUp(0))
            {
                player.animTop.SetTrigger("attack");
            }
        }
            
    }

    void IncreaseBulletSize()
    {
        timeToFire = Time.time + 1f / fireRate;
        bulletSize += speedOfIncrease;
        UIManager.Get().RefreshSizeBar();
    }

    public void Shoot()
    {
        Vector3 finalSize = new Vector3(bulletSize, bulletSize, bulletSize);
        Vector3 spawnPos = Player.Get().transform.position;
        GameObject b = Instantiate(bullet, spawnPos + player.forward.normalized * dropDistance, player.transform.rotation);
        b.transform.localScale += finalSize;

        b.GetComponent<Rigidbody>().AddForce(player.forward * shootPower * Time.fixedDeltaTime, ForceMode.Impulse);
        bulletSize = 0;
        UIManager.Get().RefreshSizeBar();
    }
}
