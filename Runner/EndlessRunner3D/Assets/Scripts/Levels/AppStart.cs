using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private int _sceneToLoad;

    private void Start()
    {
        Debug.Log("AppStart -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log("AppStart -> Initializing");

        Debug.Log($"Loading Scene: {_sceneToLoad}");
        
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneToLoad);
    }
}
