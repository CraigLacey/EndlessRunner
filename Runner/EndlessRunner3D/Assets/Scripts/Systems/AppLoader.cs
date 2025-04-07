using UnityEngine;

/// <summary>
/// AppLoader is responsible for loading all systems and managing the application lifecycle.
/// </summary>
public class AppLoader : SystemLoader
{
    /// <summary>
    /// The root transform for all MonoBehaviours that are registered in the system.
    /// </summary>
    public static Transform SystemRoot => _transform;
    private static Transform _transform;

    private static AppLoader _instance;

    private async void Awake()
    {
        // Singleton pattern to ensure only one instance of AppLoader exists
        // This is the only Singleton in the game. All other systems are registered in the ServiceLocator to provide inversion of control.
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

        // Register Systems
        RegisterSystems();

        // Register tasks to be run.
        AddStartupTasks();

        // Run tasks that have been registered.
        await RunTasks();
    }

    /// <summary>
    /// Clear static references that may be held between Playmode sessions
    /// </summary>
    private void ClearStatics()
    {
        ServiceLocator.Clear();
    }

    /// <summary>
    /// Register all systems in the game that are to be used Globally.
    /// </summary>
    private void RegisterSystems()
    {
        GameObject timerManagerGO = new GameObject("TimerManager");
        TimerManager timerManager = timerManagerGO.AddComponent<TimerManager>();
        timerManagerGO.transform.SetParent(SystemRoot);
        ServiceLocator.Register<TimerManager>(timerManager);

        GameObject scoreManagerGO = new GameObject("ScoreManager");
        ScoreManager scoreManager = scoreManagerGO.AddComponent<ScoreManager>();
        scoreManagerGO.transform.SetParent(SystemRoot);
        ServiceLocator.Register<ScoreManager>(scoreManager);

        GameObject obstacleManagerGO = new GameObject("ObstacleManager");
        ObstacleManager obstacleManager = obstacleManagerGO.AddComponent<ObstacleManager>();
        obstacleManagerGO.transform.SetParent(SystemRoot);
        ServiceLocator.Register<ObstacleManager>(obstacleManager);

        GameObject collectibleManager = new GameObject("CollectibleManager");
        CollectibleManager collectibleManagerComponent = collectibleManager.AddComponent<CollectibleManager>();
        collectibleManager.transform.SetParent(SystemRoot);
        ServiceLocator.Register<CollectibleManager>(collectibleManagerComponent);
    }

    /// <summary>
    /// Register all tasks that need to be run at startup
    /// </summary>
    private void AddStartupTasks()
    {
        Debug.Log("Register Tasks");

        // Object Pool Manager will initially load any pool info from the inspector.
        // Additional pools can be added at runtime.
        ObjectPoolManager _objectPoolManager = transform.GetComponentInChildren<ObjectPoolManager>();
        AddTask(_objectPoolManager.InitializePoolAsync);

        AddTask(ServiceLocator.Get<ObstacleManager>().InitializeAsync);
        AddTask(ServiceLocator.Get<CollectibleManager>().InitializeAsync);
    }
}
