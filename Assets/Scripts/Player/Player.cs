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

    public Transform cam;

    public bool controllable = true;

    private DiceCharacter diceLeft;
    private DiceCharacter diceRight;

    Quaternion forwardDir;
    

    private Vector3 lastInputDir = new Vector3(0f, 0f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        diceLeft = diceObjectLeft.GetComponent<DiceCharacter>();
        diceLeft.player = this;
        diceRight = diceObjectRight.GetComponent<DiceCharacter>();
        diceRight.player = this;

        Cursor.lockState = CursorLockMode.Locked;

        if(cam == null)
        {
            cam = Camera.main.transform;
        }
    }

    void FixedUpdate()
    {
        if (!controllable) 
        {
            ArmPull();
            if (!diceLeft.groundCheck()) 
            {
                diceLeft.rb.AddForce(Vector3.down * metrics.Gravity);
                
            }
            if (!diceRight.groundCheck())
            {
                diceRight.rb.AddForce(Vector3.down * metrics.Gravity);
            }
            diceLeft.rb.AddForce(-diceLeft.rb.velocity * metrics.StoppingForce);
            diceRight.rb.AddForce(-diceRight.rb.velocity * metrics.StoppingForce);
            return; 
        }

        Vector3 dir = InputDir();


        diceLeft.Movement(dir);
        diceRight.Movement(dir);

        ArmPull();

        forwardDir = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(lastInputDir.x, 0f, lastInputDir.z), Vector3.up), metrics.RotateAroundCenterDegreesPerSecond * Time.fixedDeltaTime);
        MoveToPositionAroundCenter(forwardDir);
    }

    private void Update()
    {
        //transform.position = (diceLeft.rb.position + diceRight.rb.position) * 0.5f;

        if(!controllable) { return; }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            StopAllCoroutines();
            StartCoroutine(JumpRoutine());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            InputDir();
            if (!diceLeft.ragdolling) 
            {
                diceLeft.RagdollDash(lastInputDir);
            }
            if (!diceRight.ragdolling)
            {
                diceRight.RagdollDash(lastInputDir);
            }
        }
    }

    private IEnumerator JumpRoutine()
    {
        bool infrontJumped = false;
        bool behindJumped = false;

        //Get most forward character
        DiceCharacter infront = diceLeft;
        DiceCharacter behind = diceRight;

        Vector3 input = InputDir();

        if(Vector3.Dot((diceLeft.rb.position - transform.position).normalized, input) < 0f) 
        {
            infront = diceRight;
            behind = diceLeft;
        }

        float infrontDist = Vector3.Project((infront.rb.position - transform.position), input).magnitude;

        if (infront.groundCheck(0.1f) && !infront.ragdolling) 
        {
            infrontJumped = true;
            infront.Jump();
        }


        float RemainingSecondJumpDelay = (infrontDist / metrics.MaxRunningSpeed) * 1.5f;

        while (RemainingSecondJumpDelay > 0f) 
        {
            RemainingSecondJumpDelay -= Time.deltaTime;

            if (!infrontJumped && infront.groundCheck(0.1f) && !infront.ragdolling)
            {
                infrontJumped = true;
                infront.Jump();
            }

            yield return null;
        }

        if (behind.groundCheck(0.1f) && !behind.ragdolling) 
        {
            behindJumped = true;
            behind.Jump();
        }
        
    }

    private Vector3 InputDir()
    {
        Vector3 dir = Vector3.zero;

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
            dir.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.z -= 1f;
        }

        if(dir.magnitude > 0.1f) 
        {
            Vector3 camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up);

            Debug.DrawLine(transform.position, transform.position + cam.forward, Color.yellow);

            dir = Quaternion.FromToRotation(Vector3.forward, camForward) * dir;

            Debug.DrawLine(transform.position, transform.position + dir * 1.5f, Color.blue);

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

        //Debug.DrawLine(transform.position, transform.position + rotation * Vector3.forward, Color.yellow);

        Quaternion currentPlayerRotation = Quaternion.LookRotation(Vector3.Cross((diceRight.rb.position - diceLeft.rb.position).normalized, Vector3.up));
        //Debug.DrawLine(transform.position, transform.position + currentPlayerRotation * Vector3.forward, Color.blue);

        Quaternion halfwayRot = Quaternion.Slerp(currentPlayerRotation, rotation, 0.6f);
        //Debug.DrawLine(transform.position, transform.position + halfwayRot * Vector3.forward, Color.cyan);

        transform.position = (diceLeft.rb.position + diceRight.rb.position) * 0.5f;
        float distance = (diceLeft.rb.position - diceRight.rb.position).magnitude;

        if (!diceLeft.ragdolling)
        {
            Vector3 leftPos = transform.position + Vector3.ClampMagnitude((halfwayRot * Vector3.left) * distance, metrics.ArmStretchDistance);
            Vector3 leftDelta = (leftPos - diceLeft.rb.position);
            leftDelta.y = 0;
            diceLeft.rb.AddForce(leftDelta * metrics.RotateAroundCenterForce);

            Quaternion leftTargteRotation = Quaternion.LookRotation((new Vector3(diceLeft.rb.velocity.x, 0f, diceLeft.rb.velocity.y) / metrics.MaxRunningSpeed + (rotation * Vector3.forward)).normalized, Vector3.up);
            diceLeft.rb.rotation = Quaternion.Slerp(diceLeft.rb.rotation, leftTargteRotation, metrics.rotationSpeed * Time.fixedDeltaTime);
        }

        if (!diceRight.ragdolling)
        {
            Vector3 rightPos = transform.position + Vector3.ClampMagnitude((halfwayRot * Vector3.right) * distance, metrics.ArmStretchDistance);
            Vector3 rightDelta = rightPos - diceRight.rb.position;
            rightDelta.y = 0;
            diceRight.rb.AddForce(rightDelta * metrics.RotateAroundCenterForce);

            Quaternion rightTargetRotation = Quaternion.LookRotation((new Vector3(diceRight.rb.velocity.x, 0f, diceRight.rb.velocity.y) / metrics.MaxRunningSpeed + (rotation * Vector3.forward)).normalized, Vector3.up);
            diceRight.rb.rotation = Quaternion.Slerp(diceRight.rb.rotation, rightTargetRotation, metrics.rotationSpeed * Time.fixedDeltaTime);
        }

        //Debug.DrawLine(leftPos, leftPos + Vector3.up, Color.black);
        //Debug.DrawLine(rightPos, rightPos + Vector3.up, Color.black);
    }

    public void ResetPositions() 
    {
        diceLeft.rb.position = transform.parent.position + Vector3.up * 2f - diceLeft.transform.right;
        diceRight.rb.position = transform.parent.position + Vector3.up * 2f + diceRight.transform.right;
    }
}   
