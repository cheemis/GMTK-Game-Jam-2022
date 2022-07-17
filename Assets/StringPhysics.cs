using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringPhysics : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Joints;

    [SerializeField]
    private LineRenderer rope;

    private void Start()
    {
        if(rope == null)
        {
            rope = this.GetComponent<LineRenderer>();
        }
    }
    void Update()
    {
        rope.positionCount = Joints.Length;
        for (int i = 0; i < Joints.Length; i++)
        {
            Vector3 curPos = Joints[i].transform.localPosition;
            rope.SetPosition(i, curPos);
        }
    }
}
