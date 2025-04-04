using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tiles;
    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private int _tilePosX;
    [SerializeField] private int _tilePosY;

    [SerializeField] private int _nextTilePosition = 100;
    [SerializeField] private int _tileOffsetZ = 10;

    private int _currentTileIndex = 0;

    private CollectibleManager _collectibleManager;

    public void Initialize(Action onTileExit)
    {
        _collectibleManager = ServiceLocator.Get<CollectibleManager>();

        foreach (var tileObj in _tiles)
        {
            Tile t = tileObj.GetComponent<Tile>();
            if (t != null)
            {
                t.Initialize(onTileExit);
            }
        }
    }

    /// <summary>
    /// Generates the next tile in the level.
    /// </summary>
    public void GenerateNextTile()
    {
        _tiles[_currentTileIndex].transform.position = new Vector3(_tilePosX, _tilePosY, _nextTilePosition);
        _nextTilePosition += _tileOffsetZ;
        _currentTileIndex = (_currentTileIndex + 1) % _tiles.Count;
    }

    internal void SpawnCollectibles()
    {
        foreach (var tile in _tiles)
        {
            // Get the tile's position
            Vector3 tilePos = tile.transform.position;

            // Adjust the y position to spawn above the tile
            tilePos.y += 1f;

            // Adjust the X position to be left/middle/right based on the tile index
            int randPos = UnityEngine.Random.Range(0, 100);
            if (randPos < 33)
            {
                tilePos.x = -2.5f; // Left
            }
            else if (randPos < 66)
            {
                tilePos.x = 0f; // Middle
            }
            else
            {
                tilePos.x = 2.5f; // Right
            }

            // Spawn collectibles at the tile's position
            _collectibleManager.SpawnCollectible(tilePos);
        }
    }
}
