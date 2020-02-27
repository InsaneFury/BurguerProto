using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer vid;

    void Start() {
        vid.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Skip()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
