using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{

    private void OnTriggerEnter(Collider other)
    {
        //Audio
        AkSoundEngine.PostEvent("Espada_impacto", gameObject);
    }
}
