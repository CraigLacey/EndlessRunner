using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Player Movement Stats")]
    public int MoveSpeed;
    public int LateralMoveSpeed;

    [Header("Player Movement Multiplier Stats")]
    public float MoveSpeedMultiplier;
    public float MultiplierIncreaseRate;
    public int MaxMultiplier;
}
