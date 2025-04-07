using UnityEngine;

/// <summary>
/// Gameplay class is responsible for managing the game state.
/// </summary>
public class Gameplay : MonoBehaviour
{
    [Header("Game Over Scene")]
    [SerializeField] private int _gameOverSceneIndex = 2;

    [Header("Gameplay Components")]
    [SerializeField] private Player _player;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private UIManager _uiManager;

    private TimerManager _timerManager;
    private ScoreManager _scoreManager;

    private void Start()
    {
        Debug.Log($"{nameof(Gameplay)} -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    /// <summary>
    /// Initialize the Gameplay Systems
    /// </summary>
    private void Initialize()
    {
        Debug.Log($"{nameof(Gameplay)} -> Initializing");

        // Reset Services in case we are reloading the scene
        ResetServices();

        ServiceLocator.Register<Gameplay>(this);
        
        _uiManager.Initialize();

        _timerManager = ServiceLocator.Get<TimerManager>();
        _timerManager.Initialize();
        _timerManager.TimerPhaseChange += OnTimerPhaseChange;

        _scoreManager = ServiceLocator.Get<ScoreManager>();
        _scoreManager.Initialize();

        _tileManager.Initialize();
        ServiceLocator.Register<TileManager>(_tileManager);

        _player.Initialize(OnObstacleCollision);
        ServiceLocator.Register<Player>(_player);

        Debug.Log($"{nameof(Gameplay)} -> Initialized");

        // Start Gameplay
        StartGameplay();
    }

    /// <summary>
    /// Gameplay cleanup when the scene is unloaded
    /// </summary>
    private void OnDestroy()
    {
        Debug.Log($"{nameof(Gameplay)} -> OnDestroy");
        _timerManager.TimerPhaseChange -= OnTimerPhaseChange;
    }

    /// <summary>
    /// Resets the services in case we are reloading the scene when the player retries
    /// </summary>
    private void ResetServices()
    {
        // Clear Scene Services
        ServiceLocator.Deregister<Gameplay>();
        ServiceLocator.Deregister<UIManager>();
        ServiceLocator.Deregister<TileManager>();
        ServiceLocator.Deregister<Player>();
    }

    /// <summary>
    /// Starts the gameplay by populating tiles and starting the timer, and player.
    /// </summary>
    private void StartGameplay()
    {
        _tileManager.PopulateTiles();
        _timerManager.StartTimer();
        _player.StartRunning();
    }

    /// <summary>
    /// Handles the player collision with an obstacle.
    /// </summary>
    private void OnObstacleCollision()
    {
        Debug.Log($"{nameof(Gameplay)} -> Player collided with an obstacle");

        // Handle game over logic here
        _player.StopRunning();
        _timerManager.PauseTimer();
        _tileManager.ClearTiles();

        // Clear Object Pool
        ObjectPoolManager objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
        objectPoolManager.DeactivateObjects();

        // Load GameOver Scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(_gameOverSceneIndex);
    }

    /// <summary>
    /// Handles the timer phase change event.
    /// </summary>
    private void OnTimerPhaseChange()
    {
        Debug.Log($"{nameof(Gameplay)} -> Timer Phase Change");
        _player.IncreaseSpeed();
    }
}
