using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TileGenerator _leftTileGenerator;
    [SerializeField] private TileGenerator _pathTileGenerator;
    [SerializeField] private TileGenerator _rightTileGenerator;

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
        _leftTileGenerator.GenerateNextTile();
        _pathTileGenerator.GenerateNextTile();
        _rightTileGenerator.GenerateNextTile();
    }

    internal void PopulateTiles()
    {
        _pathTileGenerator.SpawnCollectibles();
    }
}
