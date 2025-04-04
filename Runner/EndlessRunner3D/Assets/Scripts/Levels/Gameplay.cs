using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private TimerManager _timerManager;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private CollectibleManager _collectibleManager;

    private void Start()
    {
        Debug.Log($"{nameof(Gameplay)} -> Start");
        SystemLoader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Gameplay)} -> Initializing");
        ServiceLocator.Register<Gameplay>(this);

        _scoreManager.Initialize();
        ServiceLocator.Register<ScoreManager>(_scoreManager);

        _collectibleManager.Initialize();
        ServiceLocator.Register<CollectibleManager>(_collectibleManager);

        _tileManager.Initialize();
        ServiceLocator.Register<TileManager>(_tileManager);

        _timerManager.Initialize();
        ServiceLocator.Register<TimerManager>(_timerManager);

        _player.Initialize();
        ServiceLocator.Register<Player>(_player);

        Debug.Log($"{nameof(Gameplay)} -> Initialized");

        // Start Gameplay
        StartGameplay();
    }

    private void StartGameplay()
    {
        _tileManager.PopulateTiles();
        _timerManager.StartTimer();
        _player.StartRunning();
    }
}
