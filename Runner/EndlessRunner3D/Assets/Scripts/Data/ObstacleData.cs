using UnityEngine;

/// <summary>
/// ObstacleData is a class that holds data for an obstacle.
/// </summary>
[System.Serializable]
public class ObstacleData
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public int poolSize;
}
