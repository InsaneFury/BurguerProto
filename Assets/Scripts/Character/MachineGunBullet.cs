using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : Weapon
{
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Weapon") && !other.CompareTag("Ammo") && !other.CompareTag("Granade"))
        gameObject.SetActive(false);
    }
}
