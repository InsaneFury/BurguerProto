using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
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
    public Vector3 shootAngleRange;

    float timeToFire = 0f;

    Player player;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
    }

    void Start()
    {
        player = Player.Get();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1f / fireRate;
            Shoot();
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
        float direction = Random.Range(shootAngleRange[0], shootAngleRange[1]);

        GameObject currentBullet = SpawnBulletFromPool("Bullet", transform.position + player.forward.normalized, player.vision.transform.rotation);

        /*Quaternion shootAngle = Quaternion.Euler(new Vector3(currentBullet.transform.rotation.x + direction, currentBullet.transform.rotation.y, currentBullet.transform.rotation.z));
        currentBullet.transform.rotation = shootAngle;*/
        currentBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootPower * Time.fixedUnscaledDeltaTime, ForceMode.Impulse);
    }
}
