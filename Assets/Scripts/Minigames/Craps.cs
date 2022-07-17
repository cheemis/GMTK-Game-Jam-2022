using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craps : MonoBehaviour
{
    [SerializeField] private LayerMask AllowedLayers = ~0;
    [SerializeField] private List<Rigidbody> containedBodies = default;
    
    // NOTE: OnTriggerExit does not run when the script is disabled, so the bodies will still have the script attached if the craps table is turned off.
    private void OnTriggerEnter(Collider other) 
    {
        if ((AllowedLayers.value & 1 << other.gameObject.layer) != 0)
        {
            containedBodies.Add(other.attachedRigidbody);
            other.gameObject.AddComponent<DiceFaceDetection>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((AllowedLayers.value & 1 << other.gameObject.layer) != 0)
        {
            containedBodies.Remove(other.attachedRigidbody);
            Destroy(other.gameObject.GetComponent<DiceFaceDetection>());
        }
    }
}
