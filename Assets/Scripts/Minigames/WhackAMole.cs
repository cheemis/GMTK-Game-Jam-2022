using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhackAMole : MonoBehaviour
{
    [SerializeField] private float SecondsBetweenChecks = 1.0f;
    [SerializeField] private int MoleChance = 4; 
    [SerializeField] private int MolesToWin = -1;
    [SerializeField] List<GameObject> Whackamoles = default;
    [SerializeField] UnityEvent AnyGoalReachedEvent = default;
    [SerializeField] UnityEvent AllGoalsReachedEvent = default;
    
    private float _timer = 0.0f;
    private bool _timerEnabled = true;

    public void UpdateMoleCount()
    {
        if (MolesToWin == -1)
        {
            MolesToWin = Whackamoles.Count;
        }

        MolesToWin--;
        if (MolesToWin < 1)
        {
            AllGoalsReachedEvent.Invoke();
        }
    }

    private void Start() 
    {
        foreach (GameObject mole in Whackamoles)
        {
            mole.GetComponent<WhackAMoleMole>().GoalReachedEvent.AddListener(AnyGoalReachedEvent.Invoke);
        }

        AnyGoalReachedEvent.AddListener(UpdateMoleCount);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timerEnabled && _timer >= SecondsBetweenChecks)
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
