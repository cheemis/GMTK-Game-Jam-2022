using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class WhackAMoleMole : MonoBehaviour
{
    [SerializeField] private LayerMask AllowedLayers = ~0;
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
    }

    private void OnTriggerEnter(Collider other) 
    {
        // If the layer is in the layermask, go ahead with the position check.
        // If the bottom of our collider is above the top of the other collider, invoke the event.
        if ((AllowedLayers.value & 1 << other.gameObject.layer) != 0)
        {
            GoalReachedEvent.Invoke();
        }
    }
}