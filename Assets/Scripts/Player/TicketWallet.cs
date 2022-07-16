using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketWallet : MonoBehaviour
{

    public static TicketWallet inst;
    [SerializeField]
    private int ticketCount;
    [SerializeField]
    private int ticketGoal;

    private void Start()
    {
        if(inst != null)
        {
            inst = this;
            return;
        }
        Destroy(this);
    }


    public int getTicketCount()
    {
        return ticketCount;
    }
    public void addTickets(int gainTickets)
    {
        ticketCount += gainTickets;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player") && ticketCount >= ticketGoal)
        {
            Debug.Log("Play Final Cutscene");
        }
    }
}
