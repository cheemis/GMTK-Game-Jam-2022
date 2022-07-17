using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrapsTextBox : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjectsToEffect = default;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            foreach (GameObject go in gameObjectsToEffect)
            {
                go.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            foreach (GameObject go in gameObjectsToEffect)
            {
                go.SetActive(false);
            }
        }
    }
}
