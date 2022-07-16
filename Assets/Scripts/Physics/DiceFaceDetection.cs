using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFaceDetection : MonoBehaviour
{
    private Transform _transform = default;
    public int UpFace
    {
        get
        {
            float[] directions = new float[6]
            {
                Vector3.Dot( _transform.up, Vector3.up),
                Vector3.Dot( _transform.forward, Vector3.up),
                Vector3.Dot( _transform.right, Vector3.up),
                Vector3.Dot(-_transform.right, Vector3.up),
                Vector3.Dot(-_transform.forward, Vector3.up),
                Vector3.Dot(-_transform.up, Vector3.up),
            };

            int indexOfUpFace = -1;
            float compare = -1;
            for (int i = 0; i < directions.Length; i++)
            {
                if (compare < directions[i])
                {
                    compare = directions[i];
                    indexOfUpFace = i;
                }
            }

            return indexOfUpFace + 1;
        }
    }
    [SerializeField] private int currentDieValue = 0;
    private void Start() 
    {
        _transform = transform;
    }
    void Update()
    {
        currentDieValue = UpFace;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Translate(Vector3.up * 5, Space.World);
            transform.Rotate(Random.Range(0, 360.0f), Random.Range(0, 360.0f), Random.Range(0, 360.0f));
        }
    }
}
