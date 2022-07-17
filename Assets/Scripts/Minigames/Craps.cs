using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Craps : MonoBehaviour
{
    static readonly int[][] CrapSet = new int[][]
    {
        new int[] {7, 11},
        new int[] {2, 3, 12}
    };
    
    [SerializeField] private UnityEvent GoalReached = default;
    [SerializeField] private TMP_Text UIDiceTotalText = default;
    [SerializeField] private TMP_Text UICrapSetText = default;
    [SerializeField] private LayerMask AllowedLayers = ~0;
    [SerializeField] private List<Rigidbody> containedBodies = new List<Rigidbody>();
    private int[] _mySet = default;
    private bool _diceCounted = false;

    private void Start() 
    {
        if (GoalReached == null)
        {
            GoalReached = new UnityEvent();
        }

        _mySet = CrapSet[Random.Range(0, CrapSet.Length)];
        if (UICrapSetText) UICrapSetText.text = string.Join(", ", _mySet);
    }
    
    // NOTE: OnTriggerExit does not run when the script is disabled, so the bodies will still have the script attached if the craps table is turned off.
    private void OnTriggerEnter(Collider other) 
    {
        if ((AllowedLayers.value & 1 << other.gameObject.layer) != 0 && !containedBodies.Contains(other.attachedRigidbody))
        {
            containedBodies.Add(other.attachedRigidbody);
            DiceFaceDetection dfd = other.gameObject.AddComponent<DiceFaceDetection>();
            dfd.MyIndex = other.gameObject.name;
            if (UIDiceTotalText)
                dfd.UIDiceTotalText = UIDiceTotalText;
            DiceFaceDetection.DiceValues.Add(other.gameObject.name, 0);
            DiceFaceDetection.DiceValuesSet.Add(other.gameObject.name, false);
        }
        
        if (UICrapSetText) UICrapSetText.text = string.Join(", ", _mySet);
    }

    private void OnTriggerStay(Collider other) 
    {
        if (!_diceCounted && DiceFaceDetection.DiceValuesSet.Values.Count >= 2 && DiceFaceDetection.DiceValuesSet.Values.All(x=>x))
        {
            _diceCounted = true;
            bool b = _mySet.Contains(DiceFaceDetection.DiceValues.Values.Sum());
            if (UIDiceTotalText) UIDiceTotalText.color = b ? Color.green : Color.red;
            if (b) GoalReached.Invoke();
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if ((AllowedLayers.value & 1 << other.gameObject.layer) != 0 && containedBodies.Contains(other.attachedRigidbody))
        {
            containedBodies.Remove(other.attachedRigidbody);
            Destroy(other.gameObject.GetComponent<DiceFaceDetection>());
            DiceFaceDetection.DiceValues.Remove(other.gameObject.name);
            DiceFaceDetection.DiceValuesSet.Remove(other.gameObject.name);
        }

        _diceCounted = false;
        if (UIDiceTotalText) UIDiceTotalText.color = Color.white;
    }
}
