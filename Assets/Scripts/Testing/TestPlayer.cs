using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private float forceForce = default;
    private Material baseMaterial = default;
    private Vector3 startPosition = default;
    [SerializeField] private float points = 0.0f;

    private void Start() 
    {
        //baseMaterial = GetComponent<MeshRenderer>().material;
        startPosition =  transform.position;
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * forceForce, ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //GetComponent<MeshRenderer>().material = baseMaterial;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = startPosition;
        }
    }

    public void Print(string s)
    {
        Debug.Log(s);
    }

    public void AddScore(float modifier)
    {
        if (modifier > 0.0f)
        {
            points += modifier;
        }
    }
}
