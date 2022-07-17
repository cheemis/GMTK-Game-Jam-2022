using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTextContainer : MonoBehaviour
{
    [SerializeField] private GameObject PopupTextPrefab = default;
    [SerializeField] private GameObject TicketGoalPopupPrefab = default;
    public void SpawnPopupText()
    {
        Instantiate(PopupTextPrefab, transform);
    }

    public void SpawnTicketGoalPopupText()
    {
        Instantiate(TicketGoalPopupPrefab, transform);
    }
}
