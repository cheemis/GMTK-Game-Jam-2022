using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerMetrics metrics;
    [SerializeField]
    private MeshRenderer face;

    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Player player;
    private float groundOffset;

    [HideInInspector]
    public bool grounded = false;
    [HideInInspector]
    public bool ragdolling = false;
    private Vector3 groundNormal;
    private Vector3 groundPoint;
    private float slopeAngle;

    private Animator animator;
    private DiceSounds audio;

    private bool sustainedJumping = false;

    private const float minJumpSustain = 0.1f;
    private float minJumpSutainTimer;


    // Start is called before the first frame update
    void Start()
    {
        groundOffset = (transform.position - transform.GetChild(0).position).y;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audio = GetComponentInChildren<DiceSounds>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        //if (groundCheck()) 
        //{
        //    Vector3 runDirection = GetRunDirection(physCache.ForwardXZ, slopeAngle, groundPoint, groundNormal);
        //}

        if(transform.position.y < 0f) 
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
    }

    public void RagdollDash(Vector3 inputDir) 
    {
        ragdolling = true;
        animator.SetBool("Rolling", true);
        rb.constraints = 0;
        rb.angularDrag = 0.05f;
        rb.drag = 0.05f;

        ChangeFace(metrics.rollingFaces);

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        capsule.enabled = false;
        GetComponent<BoxCollider>().enabled = true;

        Vector3 force = (inputDir + Vector3.up * 0.5f).normalized  * metrics.flingForce;
        Vector3 angular = inputDir * Random.Range(12f, 25f) * (Random.Range(0f, 1f) > 0.5f ? -1f : 1f);
        angular += Vector3.Cross(inputDir, Vector3.up) * (Random.Range(90f, 200f) * -1f);

        rb.AddForce(force);
        //rb.AddTorque(angular);
        rb.angularVelocity = angular * 10f;

        audio.PlayLaunchSound();

        StopAllCoroutines();
        StartCoroutine(RagdollRoutine());
    }
    public void RagdollHit(Vector3 force) 
    {
        ragdolling = true;
        animator.SetBool("Rolling", true);
        rb.constraints = 0;
        rb.angularDrag = 0.05f;
        rb.drag = 0.05f;

        ChangeFace(metrics.rollingFaces);

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        capsule.enabled = false;
        GetComponent<BoxCollider>().enabled = true;

        Vector3 angular = force * Random.Range(12f, 25f) * (Random.Range(0f, 1f) > 0.5f ? -1f : 1f);
        angular += Vector3.Cross(force.normalized, Vector3.up) * (Random.Range(90f, 100f) * -1f);

        rb.AddForce(force);
        rb.angularVelocity = angular * 10f;

        audio.PlayJumpSound();

        StopAllCoroutines();
        StartCoroutine(RagdollRoutine());
    }

    private IEnumerator RagdollRoutine() 
    {
        float timer = metrics.maxRagdollTime;
        float velocityZeroTimer = metrics.autoStandUpDelayAfterRest;

        while(timer > 0f && velocityZeroTimer > 0f) 
        {
            timer -= Time.deltaTime;
            if(rb.angularVelocity.magnitude < 0.1f) 
            {
                velocityZeroTimer -= Time.deltaTime;
            }
            yield return null;
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.angularVelocity = Vector3.zero;
        rb.angularDrag = 0f;
        rb.drag = 0f;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(StandUpRoutine());
    }

    private IEnumerator StandUpRoutine() 
    {
        animator.SetBool("Rolling", false);
        float standUpProgress = 0f;
        while (standUpProgress > metrics.standUpDuration) 
        {
            standUpProgress += Time.deltaTime;
            yield return null;
        }

        ragdolling = false;
        ChangeFace(metrics.normalFaces);
    }

    public void Jump() 
    {
        sustainedJumping = true;
        minJumpSutainTimer = minJumpSustain;

        rb.AddForce(new Vector3(0f, metrics.JumpForce * 60f, 0f));
        audio.PlayJumpSound();
    }

    public void Movement(Vector3 inputDir) 
    {
        if (ragdolling) 
        {
            sustainedJumping = false;
            Vector3 vel = new Vector3(rb.velocity.x, (rb.velocity.y - metrics.Gravity * Time.fixedDeltaTime), rb.velocity.z);
            rb.velocity = vel;
            return;
        }

        if (groundCheck() && !sustainedJumping)
        {
            sustainedJumping = false;

            if (inputDir.magnitude == 0f)
            {
                Vector3 drag = -rb.velocity * metrics.StoppingForce;
                rb.AddForce(drag);
            }
            else
            {
                Running(inputDir);
            }

            Vector3 vel = new Vector3(rb.velocity.x, (rb.velocity.y - metrics.Gravity * Time.fixedDeltaTime * 0.1f), rb.velocity.z);
            rb.velocity = vel;
        }
        else if(sustainedJumping && Input.GetKey(KeyCode.Space)) 
        {
            AirMovement(inputDir);

            Vector3 vel = new Vector3(rb.velocity.x, (rb.velocity.y - metrics.SustainedJumpGravity * Time.fixedDeltaTime), rb.velocity.z);
            rb.velocity = vel;

            if (rb.velocity.y < 0f && minJumpSutainTimer <= 0f)
            {
                sustainedJumping = false;
            }

            minJumpSutainTimer -= Time.fixedDeltaTime;
        }
        else 
        {
            AirMovement(inputDir);

            sustainedJumping = false;
            Vector3 vel = new Vector3(rb.velocity.x, (rb.velocity.y - metrics.Gravity * Time.fixedDeltaTime), rb.velocity.z);
            rb.velocity = vel;
        }

        float speed = rb.velocity.magnitude / metrics.MaxRunningSpeed;
        if(speed > 0.05f) 
        {
            speed = Mathf.Clamp(speed, 0.5f, 2.0f);
        }
        animator.SetFloat("Speed", speed);
    }

    public bool groundCheck(float margin = 0f) 
    {
        RaycastHit info;
        if(Physics.SphereCast(transform.position, 0.55f, Vector3.down, out info, groundOffset - 0.45f + margin, metrics.groundMask)) 
        {
            groundNormal = info.normal;
            groundPoint = info.point;

            slopeAngle = Vector3.Angle(info.normal, Vector3.up);

            if(grounded == false && rb.velocity.y < -2f) 
            {
                audio.PlayFootstep();
            }

            grounded = true;
            animator.SetBool("Grounded", true);
            return true;
        }

        //Debug.DrawLine(transform.position, transform.position + Vector3.down * (groundOffset - 0.45f + 0.55f), Color.magenta);

        grounded = false;
        animator.SetBool("Grounded", false);
        return false;
    }

    public void Running(Vector3 dir)
    {
        Vector3 targetHorizontalVelocity = new Vector3(dir.x * metrics.MaxRunningSpeed, 0f, dir.z * metrics.MaxRunningSpeed);

        Vector3 currentHorizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 velocityDelta = targetHorizontalVelocity - currentHorizontalVelocity;
        float velDeltaMag = velocityDelta.magnitude;

        if (velDeltaMag > 0.01f)
        {
            Vector3 force = Vector3.ClampMagnitude(velocityDelta.normalized * metrics.GroundAcceleration, velDeltaMag * 10f);

            rb.AddForce(force);
        }



        //Debug.Log("currentHorizontalVelocity: " + currentHorizontalVelocity);
        //Debug.Log("targetHorizontalVelocity: " + targetHorizontalVelocity);
        //Debug.Log("velocityDelta: " + velocityDelta);

        Vector3 slopeVel = GetRunDirection(new Vector3(rb.velocity.x, 0f, rb.velocity.z), slopeAngle, groundPoint, groundNormal);
        rb.velocity = new Vector3(rb.velocity.x, slopeVel.y, rb.velocity.z);

        //Vector3 force = new Vector3(dir.x, 0f, dir.y) * GroundAcceleration;

        //rb.AddForce(force);

        //Vector3 clampedVelocity = new Vector3(Mathf.Clamp(rb.velocity.x, -MaxRunningSpeed, MaxRunningSpeed), Mathf.Clamp(rb.velocity.y, -1000f, 1000f), Mathf.Clamp(rb.velocity.z, -MaxRunningSpeed, MaxRunningSpeed));

        //rb.velocity = clampedVelocity;
    }

    private void AirMovement(Vector3 dir) 
    {
        Vector3 targetHorizontalVelocity = new Vector3(dir.x * metrics.MaxRunningSpeed, 0f, dir.z * metrics.MaxRunningSpeed);
        Vector3 currentHorizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 velocityDelta = targetHorizontalVelocity - currentHorizontalVelocity;
        float velDeltaMag = velocityDelta.magnitude;

        if (velDeltaMag > 0.01f)
        {
            Vector3 force = Vector3.ClampMagnitude(velocityDelta.normalized * metrics.AirAcceleration, velDeltaMag * 10f);

            rb.AddForce(force);
        }
    }

    public Vector3 GetRunDirection(Vector3 forward, float slopeAngle, Vector3 rootContactPoint, Vector3 rootContactNormal)
    {
        if (slopeAngle == 0f)
            return forward;

        float radius = 0.55f;

        Vector3 rootPos = rootContactPoint + Vector3.up * groundOffset;
        Vector3 checkPos = rootPos + forward * radius;

        if (!Physics.Linecast(rootPos, checkPos))
        {
            if (Physics.Raycast(new Ray(checkPos, Vector3.down), out RaycastHit hitInfo, GetMaxGroundHeight(slopeAngle), metrics.groundMask))
            {
                //Debug.DrawLine(rootContactPoint, rootContactPoint + hitInfo.normal, Color.red);
                rootContactNormal = Vector3.Slerp(rootContactNormal, hitInfo.normal, 0.5f);
            }
        }

        return Vector3.Cross(rootContactNormal, Vector3.Cross(forward, rootContactNormal)).normalized;
    }

    private float GetMaxGroundHeight(float slopeAngle)
    {
        float maxSlopeHeight = groundOffset + 0.15f * Mathf.Tan(metrics.MaxSlopeAngle * Mathf.Deg2Rad) - groundOffset;
        float slopeRatio = Mathf.Clamp01(slopeAngle / metrics.MaxSlopeAngle);
        return groundOffset + (maxSlopeHeight * slopeRatio) - 0.025f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ragdolling) 
        {
            audio.PlayDiceCollision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Reset") 
        {
            player.ResetPositions();
        }
    }

    private void ChangeFace(Texture2D[] textures) 
    {
        int index = Random.Range(0, textures.Length);
        face.material.SetTexture("_MainTex", textures[index]);
        face.material.SetTexture("_EmissionMap", textures[index]);
    }
}
