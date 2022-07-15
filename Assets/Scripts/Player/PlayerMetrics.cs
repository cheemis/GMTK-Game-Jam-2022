using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMetrics", menuName = "ScriptableObjects/PlayerMetrics")]
public class PlayerMetrics : ScriptableObject
{
    [Header("Ground Movement")]
    public float MaxRunningSpeed;
    public float GroundAcceleration;

    [Header("Air Movement")]
    public float JumpHeight;
    public float AirAcceleration;
    public float Gravity;
    public float AirHoldTime;

    [Header("Arms")]
    public float ArmPullForce;
    public float ArmPushForce;
    public float ArmStretchDistance;
    public float MinArmDistance;

    [Header("Orientation")]
    public float RotateAroundCenterForce;
    public float RotateAroundCenterDegreesPerSecond;

    [Header("Animation")]
    public float rotationSpeed;
}
