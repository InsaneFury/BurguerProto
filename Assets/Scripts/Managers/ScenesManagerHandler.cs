public class ScenesManagerHandler : MonobehaviourSingleton<ScenesManagerHandler>
{
    public SceneIndexes scene;

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

    public void LoadMap()
    {
        loadingManager.LoadScene(scene);
    }

}
