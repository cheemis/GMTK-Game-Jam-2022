using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnabler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
