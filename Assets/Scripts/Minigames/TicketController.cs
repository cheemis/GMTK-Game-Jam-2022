using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketController : MonoBehaviour
{
    [SerializeField] private float randomScale = 1.0f;
    [SerializeField] private GameObject TicketPrefab = default;
    [SerializeField] private AudioClip ticketSpawnSound;
    private GameObject _spawnedTicket = default;

    public void SpawnTicket()
    {
        _spawnedTicket = Instantiate(TicketPrefab, transform);
        _spawnedTicket.transform.Translate(Random.Range(0.0f, randomScale), 0, Random.Range(0.0f, randomScale));

        AudioSource.PlayClipAtPoint(ticketSpawnSound, _spawnedTicket.transform.position, 60f);
    }
}
