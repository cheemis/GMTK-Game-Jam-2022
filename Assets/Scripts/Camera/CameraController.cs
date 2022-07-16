using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float cameraTargetDistance = 15f;
    [SerializeField]
    private float targetHeightOffset = 1.2f;
    [SerializeField]
    private float smoothTime = 0.25f;
    [SerializeField]
    private float sensitivity = 1f;
    [SerializeField]
    private float collisionRadius = 1f;


    private Vector3 smoothVel;

    private Vector3 centerPositon;

    private Vector3 smoothedPosition;

    private Vector3 cameraOffsetDir = (Vector3.back * 2f + Vector3.up).normalized;

    private Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        centerPositon = target.position;
        mousePos = Input.mousePosition;
        smoothedPosition = cameraOffsetDir * cameraTargetDistance;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 mouseDelta = Input.mousePosition - mousePos;
        mousePos = Input.mousePosition;

        cameraOffsetDir = Quaternion.Euler(mouseDelta.y * sensitivity * Time.deltaTime, mouseDelta.x * sensitivity * Time.deltaTime, 0f) * cameraOffsetDir;

        Vector3 center = target.position + targetHeightOffset * Vector3.up;

        smoothedPosition = Vector3.SmoothDamp(smoothedPosition, center + cameraOffsetDir * cameraTargetDistance, ref smoothVel, smoothTime);


        Vector3 delta =  transform.position;

        //centerPositon = Vector3.SmoothDamp(centerPositon, target.position, ref smoothVel, smoothTime);

        Vector3 targetToSmoothedCam = (target.position - smoothedPosition);

        
        if(Physics.OverlapSphereNonAlloc(center, collisionRadius * 1.5f, null) > 0) 
        {
            if (Physics.OverlapSphereNonAlloc(center + targetToSmoothedCam * 0.25f, collisionRadius * 1.01f, null) > 0)
            {
                transform.position = target.position + targetToSmoothedCam * 0.2f;
            }
            else 
            {
                transform.position = targetToSmoothedCam.normalized * collisionDistance(center, 0.25f);
            }
        }
        else 
        {
            transform.position = targetToSmoothedCam.normalized * collisionDistance(center, 0f);
        }

        //transform.position = centerPositon + cameraTargetDistance * (Vector3.back * 2f + Vector3.up).normalized;

        transform.LookAt(target);
    }

    float collisionDistance(Vector3 startPos, float startFraction) 
    {
        Vector3 targetToSmoothedCam = (target.position - smoothedPosition);
        RaycastHit info;
        if (Physics.SphereCast(startPos + targetToSmoothedCam * startFraction, collisionRadius, targetToSmoothedCam.normalized, out info, cameraTargetDistance * (1f - startFraction)))
        {
            return info.distance;
        }

        return cameraTargetDistance;
    }
}
