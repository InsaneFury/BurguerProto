using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonobehaviourSingleton<MachineGun>
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Settings")]
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public float shootPower = 10f;
    public float fireRate = 0.25f;
    public float shootAngleRange;
    public int bullets = 100;
    public GameObject container;

    float timeToFire = 0f;

    Player player;
    GameManager gManager;

    public override void Awake()
    {
        base.Awake();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
    }

    void Start()
    {
        player = Player.Get();
        gManager = GameManager.Get();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.prefab.transform.position,Quaternion.identity, container.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void FixedUpdate()
    {
        if ((player.isAlive && gManager.gameStarted) && !gManager.pause)
        {
            

            if (Input.GetMouseButton(0) && Time.time >= timeToFire && (bullets > 0))
            {
                timeToFire = Time.time + 1f / fireRate;
                bullets--;

                //Audio
                AkSoundEngine.PostEvent("Mch_Gun_disparo", gameObject);

                Shoot();
                player.animMachineGun.SetBool("attack", true);
                player.animTop.SetTrigger("attack");

                //Parche previo CAMBIAR!!
                player.animBottom.SetTrigger("resetMove");
                player.animTop.SetTrigger("resetMove");

                player.muzzleFlash.Play();
            }
            if (Input.GetMouseButtonUp(0))
            {
                player.animMachineGun.SetBool("attack", false);
                player.animBottom.SetTrigger("resetMove");
                player.animTop.SetTrigger("resetMove");
                player.muzzleFlash.Stop();
            }
        }
    }

    public GameObject SpawnBulletFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rot;

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
    public void Shoot()
    {
        GameObject currentBullet = SpawnBulletFromPool("Bullet", transform.position + player.forward.normalized, player.vision.transform.rotation);

        Quaternion randRotation = Random.rotation;
        Quaternion bulletRotation = Quaternion.RotateTowards(currentBullet.transform.rotation, randRotation, shootAngleRange);
        bulletRotation.x = 0;
        bulletRotation.z = 0;
        currentBullet.transform.rotation = bulletRotation;

        currentBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * shootPower * Time.fixedUnscaledDeltaTime, ForceMode.Impulse);
    }
}
