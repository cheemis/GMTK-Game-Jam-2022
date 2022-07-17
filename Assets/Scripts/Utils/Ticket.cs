using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ticket : MonoBehaviour
{
    public int ticketCount = 1;
    [SerializeField] private UnityEvent CollectEvent = default;
    private void Start() {
        if (CollectEvent == null)
        {
            CollectEvent = new UnityEvent();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(TicketWallet.inst == null)
        {
            Debug.LogError("No Wallet loaded");
            return;
        }
        if (other.tag.Equals("Player"))
        {
            TicketWallet.inst.addTickets(ticketCount);
            CollectEvent.Invoke();
            Destroy(this.gameObject);
        }
    }
}
