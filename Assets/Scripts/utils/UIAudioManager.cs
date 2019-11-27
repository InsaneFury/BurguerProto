using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{

    public void PlayUIClick()
    {
        AkSoundEngine.PostEvent("UI_Click", gameObject);
    }

    public void PlayEasyDifficultySounds()
    {
        AkSoundEngine.SetState("Dificultad", "Facil");
    }

    public void PlayNormalDifficultySounds()
    {
        AkSoundEngine.SetState("Dificultad", "Normal");
    }

    public void PlayHardDifficultySounds()
    {
        AkSoundEngine.SetState("Dificultad", "Dificil");
    }

    public void BackToMenu()
    {
        AkSoundEngine.StopAll();

        //AkSoundEngine.PostEvent("volver_menu", gameObject);
        AkSoundEngine.PostEvent("Menu", gameObject);
    }
}
