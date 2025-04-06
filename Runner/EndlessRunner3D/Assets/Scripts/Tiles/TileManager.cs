using System.Collections;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TileGenerator _leftTileGenerator;
    [SerializeField] private TileGenerator _pathTileGenerator;
    [SerializeField] private TileGenerator _rightTileGenerator;

    WaitForSeconds _tileGenerationDelay = new WaitForSeconds(0.25f);

    public void Initialize()
    {
        Debug.Log($"{nameof(TileManager)} -> Initializing");
        _leftTileGenerator.Initialize(HandlePlayerTileExit);
        _pathTileGenerator.Initialize(HandlePlayerTileExit);
        _rightTileGenerator.Initialize(HandlePlayerTileExit);
        Debug.Log($"{nameof(TileManager)} -> Initialized");
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
}
