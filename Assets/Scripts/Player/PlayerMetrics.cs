using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMetrics", menuName = "ScriptableObjects/PlayerMetrics")]
public class PlayerMetrics : ScriptableObject
{
    [Header("Layers")]
    public LayerMask groundMask;

    [Header("Ground Movement")]
    public float MaxRunningSpeed;
    public float GroundAcceleration;
    public float MaxSlopeAngle = 45f;
    public float StoppingForce;

    [Header("Air Movement")]
    public float JumpForce;
    public float AirAcceleration;
    public float Gravity;
    public float SustainedJumpGravity;

    [Header("Arms")]
    public float ArmPullForce;
    public float ArmPushForce;
    public float ArmStretchDistance;
    public float MinArmDistance;
    public float ArmPushPullDrag;

    [Header("Orientation")]
    public float RotateAroundCenterForce;
    public float RotateAroundCenterDegreesPerSecond;

    [Header("Animation")]
    public float rotationSpeed;

    [Header("Ragdoll")]
    public float flingForce;
    public float flingAngulairForce;
    public float maxRagdollTime;
    public float autoStandUpDelayAfterRest;
    public float standUpDuration;

    [Header("Physics materials")]
    public PhysicMaterial platforming;
    public PhysicMaterial ragdolling;
}
