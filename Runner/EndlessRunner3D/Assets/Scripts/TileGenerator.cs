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

    public void Initialize(Action onTileExit)
    {
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
        Tile t = _tiles[_currentTileIndex].GetComponent<Tile>();
        _tiles[_currentTileIndex].transform.position = new Vector3(_tilePosX, _tilePosY, _nextTilePosition);
        _nextTilePosition += _tileOffsetZ;
        _currentTileIndex = (_currentTileIndex + 1) % _tiles.Count;

        // Populate the tile with items after moving it to it's new position
        if (t != null)
        {
            t.SpawnItem();
        }
    }

    public void SpawnItems()
    {
        foreach (var tileObj in _tiles)
        {
            Tile t = tileObj.GetComponent<Tile>();
            t.SpawnItem();
        }
    }

    internal void ClearTiles()
    {
        foreach(var tileObj in _tiles)
        {
            Tile t = tileObj.GetComponent<Tile>();
            t.ClearItems();
        }
    }
}
