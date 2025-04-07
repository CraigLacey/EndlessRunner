using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _tileObjects;
    [SerializeField] private int _tilePosX;
    [SerializeField] private int _tilePosY;
    [SerializeField] private int _nextTilePosition = 100;
    [SerializeField] private int _tileOffsetZ = 10;

    private int _currentTileIndex = 0;
    private List<Tile> _tiles = new();

    public void Initialize(Action onTileExit)
    {
        foreach (var tileObj in _tileObjects)
        {
            Tile t = tileObj.GetComponent<Tile>();
            if (t != null)
            {
                t.Initialize(onTileExit);
                _tiles.Add(t);
            }
        }
    }

    /// <summary>
    /// Generates the next tile in the level.
    /// </summary>
    public void GenerateNextTile()
    {
        Tile t = _tileObjects[_currentTileIndex].GetComponent<Tile>();
        _tileObjects[_currentTileIndex].transform.position = new Vector3(_tilePosX, _tilePosY, _nextTilePosition);
        _nextTilePosition += _tileOffsetZ;
        _currentTileIndex = (_currentTileIndex + 1) % _tileObjects.Count;

        // Populate the tile with items after moving it to it's new position
        if (t != null)
        {
            t.SpawnItem();
        }
    }

    public void SpawnItems()
    {
        foreach (var tile in _tiles)
        {
            tile.SpawnItem();
        }
    }

    internal void ClearTiles()
    {
        foreach(var tile in _tiles)
        {
            tile.ClearItems();
        }
    }

    internal void IncreaseObstacleSpawnRate()
    {
        foreach (var tile in _tiles)
        {
            tile.IncreaseObstacleSpawnChance();
        }
    }
}
