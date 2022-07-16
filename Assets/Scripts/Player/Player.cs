using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMetrics metrics;

    [SerializeField]
    private GameObject diceLeft;
    [SerializeField]
    private GameObject diceRight;

    [SerializeField]
    private Transform cam;

    private Rigidbody rbLeft;
    private Rigidbody rbRight;

    private Vector2 lastInputDir = new Vector2(0f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        rbLeft = diceLeft.GetComponent<Rigidbody>();
        rbRight = diceRight.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector2 dir = InputDir();
        

        Running(rbLeft, dir);
        Running(rbRight, dir);

        ArmPull();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(lastInputDir.x, 0f, lastInputDir.y), Vector3.up), metrics.RotateAroundCenterDegreesPerSecond * Time.fixedDeltaTime);
        MoveToPositionAroundCenter(transform.rotation);
    }

    private void Update()
    {
        transform.position = (rbLeft.position + rbRight.position) * 0.5f;
    }

    private void Running(Rigidbody rb, Vector2 dir) 
    {
        Vector3 targetVelocity = new Vector3(dir.x * metrics.MaxRunningSpeed, rb.velocity.y, dir.y * metrics.MaxRunningSpeed);

        Vector3 velocityDelta = targetVelocity - rb.velocity;
        float velcDeltaMag = velocityDelta.magnitude;

        if(velcDeltaMag > 0.01f) 
        {
            Vector3 force = Vector3.ClampMagnitude(velocityDelta.normalized * metrics.GroundAcceleration, velcDeltaMag * 10f);

            rb.AddForce(force);
        }

        Debug.Log("targetVelocity: " + targetVelocity);
        Debug.Log("velocityDelta: " + velocityDelta);


        //Vector3 force = new Vector3(dir.x, 0f, dir.y) * GroundAcceleration;

        //rb.AddForce(force);

        //Vector3 clampedVelocity = new Vector3(Mathf.Clamp(rb.velocity.x, -MaxRunningSpeed, MaxRunningSpeed), Mathf.Clamp(rb.velocity.y, -1000f, 1000f), Mathf.Clamp(rb.velocity.z, -MaxRunningSpeed, MaxRunningSpeed));

        //rb.velocity = clampedVelocity;
    }

    private Vector2 InputDir()
    {
        //TODO: take into account camera dir

        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            dir.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x += 1f;
        }
        if (Input.GetKey(KeyCode.W)) 
        {
            dir.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y -= 1f;
        }

        if(dir.magnitude > 0.1f) 
        {
            lastInputDir = dir.normalized;
            return lastInputDir;
        }

        return dir;
    }

    private void ArmPull() 
    {
        Vector3 AtoB = rbRight.position - rbLeft.position;
        float distance = AtoB.magnitude;

        if (distance > metrics.ArmStretchDistance) 
        {
            float force = (distance - metrics.ArmStretchDistance) * metrics.ArmPullForce;
            AtoB.Normalize();
            rbLeft.AddForce(AtoB * force);
            rbRight.AddForce(-AtoB * force);
        }
        else if (distance < metrics.MinArmDistance)
        {
            float force = Mathf.Abs(distance - metrics.MinArmDistance) * metrics.ArmPushForce;
            AtoB.Normalize();
            rbLeft.AddForce(-AtoB * force);
            rbRight.AddForce(AtoB * force);
        }
        
    }

    private void MoveToPositionAroundCenter(Quaternion rotation) 
    {
        Debug.DrawLine(transform.position, transform.position + rotation * Vector3.forward, Color.yellow);

        Quaternion currentPlayerRotation = Quaternion.LookRotation(Vector3.Cross((rbRight.position - rbLeft.position).normalized, Vector3.up));
        Debug.DrawLine(transform.position, transform.position + currentPlayerRotation * Vector3.forward, Color.blue);

        Quaternion halfwayRot = Quaternion.Slerp(currentPlayerRotation, rotation, 0.6f);
        Debug.DrawLine(transform.position, transform.position + halfwayRot * Vector3.forward, Color.cyan);

        transform.position = (rbLeft.position + rbRight.position) * 0.5f;
        float distance = (rbLeft.position - rbRight.position).magnitude;

        Vector3 leftPos = transform.position + Vector3.ClampMagnitude((halfwayRot * Vector3.left) * distance, metrics.ArmStretchDistance);
        Vector3 leftDelta =  (leftPos - rbLeft.position);
        leftDelta.y = 0;
        rbLeft.AddForce(leftDelta * metrics.RotateAroundCenterForce);

        Vector3 rightPos = transform.position + Vector3.ClampMagnitude((halfwayRot * Vector3.right) * distance, metrics.ArmStretchDistance);
        Vector3 rightDelta = rightPos - rbRight.position;
        rightDelta.y = 0;
        rbRight.AddForce(rightDelta * metrics.RotateAroundCenterForce);

        Debug.DrawLine(leftPos, leftPos + Vector3.up, Color.black);
        Debug.DrawLine(rightPos, rightPos + Vector3.up, Color.black);

        //SetForward directions
        Quaternion leftTargteRotation = Quaternion.LookRotation((new Vector3(rbLeft.velocity.x, 0f, rbLeft.velocity.y) / metrics.MaxRunningSpeed + (rotation * Vector3.forward) * 0.2f).normalized, Vector3.up);
        rbLeft.rotation = Quaternion.Slerp(rbLeft.rotation, leftTargteRotation, metrics.rotationSpeed * Time.fixedDeltaTime);

        Quaternion rightTargetRotation = Quaternion.LookRotation((new Vector3(rbRight.velocity.x, 0f, rbRight.velocity.y) / metrics.MaxRunningSpeed + (rotation * Vector3.forward) * 0.2f).normalized, Vector3.up);
        rbRight.rotation = Quaternion.Slerp(rbRight.rotation, rightTargetRotation, metrics.rotationSpeed * Time.fixedDeltaTime);
    }
}   
