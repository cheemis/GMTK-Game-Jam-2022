using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    public int ticketCount = 1;
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
            Destroy(this.gameObject);
        }
    }
}
