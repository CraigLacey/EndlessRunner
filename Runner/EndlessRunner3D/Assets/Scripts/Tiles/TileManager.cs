using System;
using System.Collections;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TileGenerator _leftTileGenerator;
    [SerializeField] private TileGenerator _pathTileGenerator;
    [SerializeField] private TileGenerator _rightTileGenerator;

    private WaitForSeconds _tileGenerationDelay = new WaitForSeconds(0.25f);
    private TimerManager _timerManager;

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

    private void OnTimerPhaseChange()
    {
        _pathTileGenerator.IncreaseObstacleSpawnRate();
    }

    private void HandlePlayerTileExit()
    {
        StartCoroutine(GenerateTilesAsync());
    }

    public void PopulateTiles()
    {
        _leftTileGenerator.SpawnItems();
        _pathTileGenerator.SpawnItems();
        _rightTileGenerator.SpawnItems();
    }

    private IEnumerator GenerateTilesAsync()
    {
        yield return _tileGenerationDelay;
        _leftTileGenerator.GenerateNextTile();
        _pathTileGenerator.GenerateNextTile();
        _rightTileGenerator.GenerateNextTile();
    }

    internal void ClearTiles()
    {
        _leftTileGenerator.ClearTiles();
        _pathTileGenerator.ClearTiles();
        _rightTileGenerator.ClearTiles();
    }
}
