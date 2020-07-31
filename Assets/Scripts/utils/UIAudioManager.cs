using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{

    public void PlayUIClick()
    {
        AkSoundEngine.PostEvent("ui_button_click", gameObject);
    }

    public void BackToMenu()
    {
        //AkSoundEngine.StopAll();

        AkSoundEngine.PostEvent("volver_menu", gameObject);
        AkSoundEngine.PostEvent("Menu", gameObject);
    }

}
