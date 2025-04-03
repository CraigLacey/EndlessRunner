using UnityEditor;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private SceneAsset _sceneToLoad;

    private void Start()
    {
        Debug.Log("AppStart -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log("AppStart -> Initializing");
        if (_sceneToLoad == null)
        {
            Debug.LogError("Scene To Load not found, check inspector");
            return;
        }

        Debug.Log($"Loading Scene: {_sceneToLoad.name}");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneToLoad.name);
    }
}
