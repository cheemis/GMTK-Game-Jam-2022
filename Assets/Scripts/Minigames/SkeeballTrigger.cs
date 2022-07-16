using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SkeeballTrigger : MonoBehaviour
{
    //[SerializeField] private LayerMask AllowedLayers = default;
    private Skeeball _parent = default;
    public UnityEvent GoalReachedEvent = default;

    private Collider _thisCollider = default;

    private void Start() 
    {
        // Initialize the event and collider if they aren't already.
        if (GoalReachedEvent == null)
        {
            GoalReachedEvent = new UnityEvent();
        }
        _thisCollider = GetComponent<Collider>();
        _parent = transform.parent.GetComponent<Skeeball>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        // If the other is in our layermask, invoke the event.
        if ((_parent.AllowedLayers.value & 1 << other.gameObject.layer) != 0)
        {
            GoalReachedEvent.Invoke();
        }
    }
}
