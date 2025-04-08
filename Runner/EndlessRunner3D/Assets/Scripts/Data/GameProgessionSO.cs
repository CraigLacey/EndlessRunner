using UnityEngine;

[CreateAssetMenu(fileName = "GameProgessionSO", menuName = "ScriptableObjects/GameProgession", order = 1)]
public class GameProgessionSO : ScriptableObject
{
    [Header("Game Phase Progression")]
    [Tooltip("Time length of each phase. After a phase the game difficulty will increase")]
    public float PhaseDurationSeconds;

    [Header("Obstacle Spawn Rate Stats")]
    [Tooltip("The percent chance of spawning an obstacle on a tile")]
    [Range(10f, 50f)]
    public int ObstacleSpawnChance;

    [Tooltip("The amount to increase the obstacle spawn chance by each time")]
    [Range(1f, 20f)]
    public int ObstacleChanceIncrement;


    [Tooltip("The maximum chance of spawning an obstacle on a tile")]
    [Range(10f, 100f)]
    public int MaxObstacleChance;
}
