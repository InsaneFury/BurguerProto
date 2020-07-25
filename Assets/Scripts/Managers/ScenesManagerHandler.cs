public class ScenesManagerHandler : MonobehaviourSingleton<ScenesManagerHandler>
{
    LoadingScenesManager loadingManager;

    public override void Awake()
    {
        base.Awake();
        loadingManager = FindObjectOfType<LoadingScenesManager>();
    }
    
    public void LoadSceneHandler(int sceneToLoad)
    {
        loadingManager.LoadScene((SceneIndexes)sceneToLoad);
    }
}
