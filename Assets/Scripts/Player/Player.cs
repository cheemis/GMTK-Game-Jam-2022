using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerMetrics metrics;

    [SerializeField]
    private GameObject diceObjectLeft;
    [SerializeField]
    private GameObject diceObjectRight;

    [SerializeField]
    private Transform cam;

    private DiceCharacter diceLeft;
    private DiceCharacter diceRight;
    

    private Vector2 lastInputDir = new Vector2(0f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        diceLeft = diceObjectLeft.GetComponent<DiceCharacter>();
        diceRight = diceObjectRight.GetComponent<DiceCharacter>();
    }

    void FixedUpdate()
    {
        Vector2 dir = InputDir();


        diceLeft.Movement(dir);
        diceRight.Movement(dir);

        ArmPull();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(lastInputDir.x, 0f, lastInputDir.y), Vector3.up), metrics.RotateAroundCenterDegreesPerSecond * Time.fixedDeltaTime);
        MoveToPositionAroundCenter(transform.rotation);
    }

    private void Update()
    {
        transform.position = (diceLeft.rb.position + diceRight.rb.position) * 0.5f;

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StartCoroutine(JumpRoutine());
        }
    }

    private IEnumerator JumpRoutine()
    {
        bool infrontJumped = false;
        bool behindJumped = false;

        //Get most forward character
        DiceCharacter infront = diceLeft;
        DiceCharacter behind = diceRight;

        Vector2 input = InputDir();
        Vector3 inputForward = new Vector3(input.x, 0f, input.y);

        if(Vector3.Dot((diceLeft.rb.position - transform.position).normalized, inputForward) < 0f) 
        {
            infront = diceRight;
            behind = diceLeft;
        }

        float infrontDist = Vector3.Project((infront.rb.position - transform.position), inputForward).magnitude;

        if (infront.grounded) 
        {
            infrontJumped = true;
            infront.Jump();
        }


        float RemainingSecondJumpDelay = (infrontDist / metrics.MaxRunningSpeed) * 1.5f;

        while (RemainingSecondJumpDelay > 0f) 
        {
            RemainingSecondJumpDelay -= Time.deltaTime;

            if (!infrontJumped && infront.grounded)
            {
                infrontJumped = true;
                infront.Jump();
            }

            yield return null;
        }

        if (behind.grounded) 
        {
            behindJumped = true;
            behind.Jump();
        }
        
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
        Vector3 AtoB = diceRight.rb.position - diceLeft.rb.position;
        float distance = AtoB.magnitude;

        if (distance > metrics.ArmStretchDistance) 
        {
            float force = (distance - metrics.ArmStretchDistance) * metrics.ArmPullForce;
            AtoB.Normalize();
            diceLeft.rb.AddForce(AtoB * force);
            diceRight.rb.AddForce(-AtoB * force);
        }
        else if (distance < metrics.MinArmDistance)
        {
            float force = Mathf.Abs(distance - metrics.MinArmDistance) * metrics.ArmPushForce;
            AtoB.Normalize();
            diceLeft.rb.AddForce(-AtoB * force);
            diceRight.rb.AddForce(AtoB * force);
        }

        Vector3 relativeVel = diceRight.rb.velocity - diceLeft.rb.velocity;
        diceLeft.rb.AddForce(relativeVel * metrics.ArmPushPullDrag);
        diceRight.rb.AddForce(-relativeVel * metrics.ArmPushPullDrag);
    }

    private void MoveToPositionAroundCenter(Quaternion rotation) 
    {
        Debug.DrawLine(transform.position, transform.position + rotation * Vector3.forward, Color.yellow);

        Quaternion currentPlayerRotation = Quaternion.LookRotation(Vector3.Cross((diceRight.rb.position - diceLeft.rb.position).normalized, Vector3.up));
        Debug.DrawLine(transform.position, transform.position + currentPlayerRotation * Vector3.forward, Color.blue);

        Quaternion halfwayRot = Quaternion.Slerp(currentPlayerRotation, rotation, 0.6f);
        Debug.DrawLine(transform.position, transform.position + halfwayRot * Vector3.forward, Color.cyan);

        transform.position = (diceLeft.rb.position + diceRight.rb.position) * 0.5f;
        float distance = (diceLeft.rb.position - diceRight.rb.position).magnitude;

        Vector3 leftPos = transform.position + Vector3.ClampMagnitude((halfwayRot * Vector3.left) * distance, metrics.ArmStretchDistance);
        Vector3 leftDelta =  (leftPos - diceLeft.rb.position);
        leftDelta.y = 0;
        diceLeft.rb.AddForce(leftDelta * metrics.RotateAroundCenterForce);

        Vector3 rightPos = transform.position + Vector3.ClampMagnitude((halfwayRot * Vector3.right) * distance, metrics.ArmStretchDistance);
        Vector3 rightDelta = rightPos - diceRight.rb.position;
        rightDelta.y = 0;
        diceRight.rb.AddForce(rightDelta * metrics.RotateAroundCenterForce);

        Debug.DrawLine(leftPos, leftPos + Vector3.up, Color.black);
        Debug.DrawLine(rightPos, rightPos + Vector3.up, Color.black);

        //SetForward directions
        Quaternion leftTargteRotation = Quaternion.LookRotation((new Vector3(diceLeft.rb.velocity.x, 0f, diceLeft.rb.velocity.y) / metrics.MaxRunningSpeed + (rotation * Vector3.forward) * 0.2f).normalized, Vector3.up);
        diceLeft.rb.rotation = Quaternion.Slerp(diceLeft.rb.rotation, leftTargteRotation, metrics.rotationSpeed * Time.fixedDeltaTime);

        Quaternion rightTargetRotation = Quaternion.LookRotation((new Vector3(diceRight.rb.velocity.x, 0f, diceRight.rb.velocity.y) / metrics.MaxRunningSpeed + (rotation * Vector3.forward) * 0.2f).normalized, Vector3.up);
        diceRight.rb.rotation = Quaternion.Slerp(diceRight.rb.rotation, rightTargetRotation, metrics.rotationSpeed * Time.fixedDeltaTime);
    }

}   
