using UnityEngine;

public class AppLoader : SystemLoader
{
    public static Transform SystemRoot => _transform;
    private static Transform _transform;

    private static AppLoader _instance;

    private async void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // Duplicate loader detected
            gameObject.SetActive(false);
            return;
        }
        _instance = this;
        _transform = transform;
        DontDestroyOnLoad(gameObject);

        // Clear any statics that may have been held between Playmode sessions.
        ClearStatics();

        // Load the application config.
        LoadConfig();

        // Register tasks to be run.
        RegisterTasks();

        // Run tasks that have been registered.
        await RunTasks();
    }

    private void ClearStatics()
    {
        ServiceLocator.Clear();
    }

    private void LoadConfig()
    {
        Debug.Log("Loading Config");
    }

    private void RegisterTasks()
    {
        Debug.Log("Register Tasks");

        ObjectPoolManager _objectPoolManager = transform.GetComponentInChildren<ObjectPoolManager>();
        AddTask(_objectPoolManager.InitializePoolAsync);
    }
}
