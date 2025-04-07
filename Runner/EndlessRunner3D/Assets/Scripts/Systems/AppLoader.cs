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

        // Register Systems
        RegisterSystems();

        // Register tasks to be run.
        AddStartupTasks();

        // Run tasks that have been registered.
        await RunTasks();
    }

    private void ClearStatics()
    {
        ServiceLocator.Clear();
    }

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

    private void AddStartupTasks()
    {
        Debug.Log("Register Tasks");

        // Object Pool Manager will load data from disk into ObjectPool
        ObjectPoolManager _objectPoolManager = transform.GetComponentInChildren<ObjectPoolManager>();
        AddTask(_objectPoolManager.InitializePoolAsync);

        AddTask(ServiceLocator.Get<ObstacleManager>().InitializeAsync);
        AddTask(ServiceLocator.Get<CollectibleManager>().InitializeAsync);
    }
}
