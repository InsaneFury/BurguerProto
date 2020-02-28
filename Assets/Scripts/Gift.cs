using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public GameObject bullets;
    public GameObject granades;
    public float respawnSeconds = 20;

    void Drop()
    {
        Instantiate(bullets,transform.position, Quaternion.identity);
        Instantiate(granades, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Drop();
            StartCoroutine(ReactiveGift());
        }
    }

    IEnumerator ReactiveGift()
    {
        gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(respawnSeconds);
        gameObject.SetActive(true);
    }
}
