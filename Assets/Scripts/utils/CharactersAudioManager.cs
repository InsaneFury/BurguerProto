using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersAudioManager : MonoBehaviour
{
    public void PlayEnemyRunningSound()
    {
        AkSoundEngine.PostEvent("Mov_enemigos", gameObject);
    }

    public void PlayPlayerWalkCycleSound()
    {
        AkSoundEngine.PostEvent("Mov_hamburguesa", gameObject);
    }
}
