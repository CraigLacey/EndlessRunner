using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private TimerManager _timerManager;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private CollectibleManager _collectibleManager;
    [SerializeField] private ObstacleManager _obstacleManager;

    private void Start()
    {
        Debug.Log($"{nameof(Gameplay)} -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Gameplay)} -> Initializing");

        // Reset Services in case we are reloading the scene
        ResetServices();

        ServiceLocator.Register<Gameplay>(this);

        _scoreManager.Initialize();
        ServiceLocator.Register<ScoreManager>(_scoreManager);

        _collectibleManager.Initialize();
        ServiceLocator.Register<CollectibleManager>(_collectibleManager);

        _obstacleManager.Initialize();
        ServiceLocator.Register<ObstacleManager>(_obstacleManager);

        _tileManager.Initialize();
        ServiceLocator.Register<TileManager>(_tileManager);

        _timerManager.Initialize();
        ServiceLocator.Register<TimerManager>(_timerManager);

        _player.Initialize(OnObstacleCollision);
        ServiceLocator.Register<Player>(_player);

        Debug.Log($"{nameof(Gameplay)} -> Initialized");

        // Start Gameplay
        StartGameplay();
    }

    private void ResetServices()
    {
        // Clear Scene Services
        ServiceLocator.Deregister<Gameplay>();
        ServiceLocator.Deregister<ScoreManager>();
        ServiceLocator.Deregister<CollectibleManager>();
        ServiceLocator.Deregister<ObstacleManager>();
        ServiceLocator.Deregister<TileManager>();
        ServiceLocator.Deregister<TimerManager>();
        ServiceLocator.Deregister<Player>();
    }

    private void StartGameplay()
    {
        _tileManager.PopulateTiles();
        _timerManager.StartTimer();
        _player.StartRunning();
    }

    private void OnObstacleCollision()
    {
        Debug.Log($"{nameof(Gameplay)} -> Player collided with an obstacle");
        
        // Handle game over logic here
        _player.StopRunning();
        _timerManager.PauseTimer();
        
        // Clear Object Pool
        ObjectPoolManager objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        objectPoolManager.DeactivateObjects();

        // Load GameOver Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
