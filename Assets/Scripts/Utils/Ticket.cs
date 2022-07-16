using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    public int ticketCount = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            TicketWallet.inst.addTickets(ticketCount);
            Destroy(this.gameObject);
        }
    }
}
