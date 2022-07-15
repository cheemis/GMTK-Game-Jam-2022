using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private float forceForce = default;
    [SerializeField] private Material baseMaterial = default;

    private void Start() {
        baseMaterial = GetComponent<MeshRenderer>().material;
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * forceForce, ForceMode.Force);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<MeshRenderer>().material = baseMaterial;
        }
    }

    public void Print(string s)
    {
        Debug.Log(s);
    }
}
