using System.Threading.Tasks;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private int _sceneToLoad;

    private float _minDisplayTime = 2.0f * 1000;

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
        var loadTask = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneToLoad);
        while(loadTask.isDone == false)
        {
            Debug.Log($"Loading Scene: {_sceneToLoad} - Progress: {loadTask.progress * 100}%");
            await Task.Delay(100);
        }
    }

    private async Task ShowSplashScreenAsync()
    {
        await Task.Delay((int)_minDisplayTime);
    }
}
