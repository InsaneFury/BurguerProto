using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int minDamage = 5;
    public int maxDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        //Audio
        AkSoundEngine.PostEvent("Espada_impacto", gameObject);

        if (other.CompareTag("Enemy"))
        {
            int randDmg = (int)Random.Range(minDamage, maxDamage);
            other.gameObject.GetComponent<Enemy>().TakeDamage(randDmg);
        }
    }
}
