using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhackAMole : MonoBehaviour
{
    [SerializeField] private float SecondsBetweenChecks = 1.0f;
    private float _timer = 0.0f;
    [SerializeField] private int MoleChance = 4; 
    [SerializeField] List<GameObject> Whackamoles = default;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= SecondsBetweenChecks)
        {
            _timer = 0.0f;
            if (Random.Range(1, MoleChance) == 1)
            {
                int index = Random.Range(0, Whackamoles.Count);
                for (int i = 0; i < Whackamoles.Count; index = (index + 1) % Whackamoles.Count)
                {
                    if (Whackamoles[index].activeInHierarchy)
                    {
                        i++;
                        continue;
                    }
                    Whackamoles[index].SetActive(true);
                    break;
                }
            }
        }
    }
}
