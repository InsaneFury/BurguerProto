using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSize;
    public float speedOfIncrease = 0.1f;
    public float dropDistance = 1;
    public float shootPower = 10f;
    public float fireRate = 2f;

    Player player;
    float timer = 0;
    float timeToFire = 0f;

    void Start()
    {
        bulletSize = bullet.transform.localScale.x;
        player = Player.Get();

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && (Time.time >= timeToFire))
        {
            timeToFire = Time.time + 1f / fireRate;
            bulletSize += speedOfIncrease;
            Debug.Log(bulletSize);
            timer = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 finalSize = new Vector3(bulletSize, bulletSize, bulletSize);
            Vector3 spawnPos = Player.Get().transform.position;
            GameObject b = Instantiate(bullet, spawnPos + player.forward.normalized * dropDistance, player.transform.rotation);
            b.transform.localScale += finalSize;

            b.GetComponent<Rigidbody>().AddForce(player.forward * shootPower * Time.fixedDeltaTime, ForceMode.Impulse);

            bulletSize = bullet.transform.localScale.x;
        }
    }
}
