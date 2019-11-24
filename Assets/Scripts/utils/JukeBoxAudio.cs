﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBoxAudio : MonobehaviourSingleton<JukeBoxAudio>
{

    public override void Awake()
    {
        base.Awake();
    }

    public void PlayJukeBoxAudio()
    {
        AkSoundEngine.PostEvent("Rocola_Start", gameObject);
    }

    public void StopJukeBoxAudio()
    {
        AkSoundEngine.PostEvent("Rocola_Stop", gameObject);
    }

    public void DamageJukeBoxAudio()
    {
        AkSoundEngine.PostEvent("Rocola_damage", gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Explosion"))
        {
            DamageJukeBoxAudio();
        }
    }

}
