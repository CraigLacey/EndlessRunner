using System.Collections;
using UnityEngine;

/// <summary>
/// TileManager is responsible for managing the TileGenerators. There is a TileGenerator for each lane.
/// </summary>
public class TileManager : MonoBehaviour
{
    [Header("Tile Generators")]
    [SerializeField] private TileGenerator _leftTileGenerator;
    [SerializeField] private TileGenerator _pathTileGenerator;
    [SerializeField] private TileGenerator _rightTileGenerator;

    private WaitForSeconds _tileGenerationDelay = new WaitForSeconds(0.25f);
    private TimerManager _timerManager;

    /// <summary>
    /// Initializes the TileManager and its TileGenerators.
    /// </summary>
    public void Initialize()
    {
        Debug.Log($"{nameof(TileManager)} -> Initializing");

        _leftTileGenerator.Initialize(HandlePlayerTileExit);
        _pathTileGenerator.Initialize(HandlePlayerTileExit);
        _rightTileGenerator.Initialize(HandlePlayerTileExit);

        _timerManager = ServiceLocator.Get<TimerManager>();
        _timerManager.TimerPhaseChange += OnTimerPhaseChange;

        Debug.Log($"{nameof(TileManager)} -> Initialized");
    }

    /// <summary>
    /// Handles what happens when the timer phase changes
    /// </summary>
    private void OnTimerPhaseChange()
    {
        _pathTileGenerator.IncreaseObstacleSpawnRate();
    }

    /// <summary>
    /// Handles the logic when the player exits a tile.
    /// </summary>
    private void HandlePlayerTileExit()
    {
        StartCoroutine(GenerateTilesAsync());
    }

    /// <summary>
    /// Populates the tiles with items. Called when the game starts to set the initial state of the tiles.
    /// </summary>
    public void PopulateTiles()
    {
        _leftTileGenerator.SpawnItems();
        _pathTileGenerator.SpawnItems();
        _rightTileGenerator.SpawnItems();
    }

    /// <summary>
    /// Clears the tiles. This is called when the game is reset or restarted. Or when a Tile is exited and needs to be cleared.
    /// </summary>
    public void ClearTiles()
    {
        _leftTileGenerator.ClearTiles();
        _pathTileGenerator.ClearTiles();
        _rightTileGenerator.ClearTiles();
    }

    /// <summary>
    /// Generates the next set of tiles asynchronously. This is called when the player exits a tile to generate the next tile.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateTilesAsync()
    {
        yield return _tileGenerationDelay;
        _leftTileGenerator.GenerateNextTile();
        _pathTileGenerator.GenerateNextTile();
        _rightTileGenerator.GenerateNextTile();
    }
}
