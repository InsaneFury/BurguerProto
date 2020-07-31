using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CharactersAudioManager : MonoBehaviour
{
    ScenesManagerHandler levelsManager;

    private void Start()
    {
        levelsManager = ScenesManagerHandler.Get();
        if (levelsManager.scene == SceneIndexes.LEVEL_1)
        {
            AkSoundEngine.SetSwitch("Surface", "concrete", gameObject);
        }
        else
        {
            AkSoundEngine.SetSwitch("Surface", "snow", gameObject);
        }
            
    }
    public void PlayEnemyRunningSound()
    {
        AkSoundEngine.PostEvent("enemy_walk", gameObject);
    }

    public void PlayPlayerWalkCycleSound()
    {
        AkSoundEngine.PostEvent("player_walk", gameObject);
    }
}
