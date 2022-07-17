using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketController : MonoBehaviour
{
    [SerializeField] private GameObject TicketPrefab = default;
    private GameObject _spawnedTicket = default;

    public void SpawnTicket()
    {
        _spawnedTicket = Instantiate(TicketPrefab, transform);
    }
}
