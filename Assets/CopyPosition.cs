using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{

    //Better than parent for weird usecases of locality
    [SerializeField]
    private GameObject copyPosition;
    void Update()
    {
        if(copyPosition != null)
        {
            this.transform.position = copyPosition.transform.position;
        }
    }
}
