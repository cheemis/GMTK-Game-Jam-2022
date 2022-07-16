using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skeeball : MonoBehaviour
{
    [SerializeField] private LayerMask allowedLayers = ~0;
    public LayerMask AllowedLayers 
    {
        get
        {
            return allowedLayers;
        }
    }
    public UnityEvent AnyGoalReachedEvent = default;

    private List<SkeeballTrigger> _triggers = new List<SkeeballTrigger>(); 

    private void Start() 
    {
        // Initialize the event and collider if they aren't already.
        if (AnyGoalReachedEvent == null)
        {
            AnyGoalReachedEvent = new UnityEvent();
        }

        // Search in our children for Skeeball triggers and add them to the list.
        foreach (Transform c in transform)
        {
            SkeeballTrigger sbt = c.GetComponent<SkeeballTrigger>();
            if (sbt)
            {
                _triggers.Add(sbt);
            }
        }

        foreach (SkeeballTrigger s in _triggers)
        {
            s.GoalReachedEvent.AddListener(AnyGoalReachedEvent.Invoke);
        }
    }
}
