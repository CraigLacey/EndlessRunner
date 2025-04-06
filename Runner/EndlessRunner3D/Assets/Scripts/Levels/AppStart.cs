using System.Threading.Tasks;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private int _sceneToLoad;

    private float _minDisplayTime = 3.5f * 1000;

    private void Start()
    {
        Debug.Log("AppStart -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    private async void Initialize()
    {
        Debug.Log("AppStart -> Initializing");

        Debug.Log($"Loading Scene: {_sceneToLoad}");
        await ShowSplashScreenAsync();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneToLoad);
    }

    private async Task ShowSplashScreenAsync()
    {
        await Task.Delay((int)_minDisplayTime);
    }
}
