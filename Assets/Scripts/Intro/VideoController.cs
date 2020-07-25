using UnityEngine;
using UnityEngine.Video;
public class VideoController : MonoBehaviour
{
    public VideoPlayer vid;
    private ScenesManagerHandler sceneHandler;

    private void Awake() => sceneHandler = ScenesManagerHandler.Get();
    void Start() => vid.loopPointReached += CheckOver;
    void CheckOver(UnityEngine.Video.VideoPlayer vp) => sceneHandler.LoadSceneHandler((int)SceneIndexes.MENU);
    public void Skip() => sceneHandler.LoadSceneHandler((int)SceneIndexes.MENU);
    
}
