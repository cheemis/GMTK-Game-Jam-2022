using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TicketWallet : MonoBehaviour
{
    [SerializeField] private UnityEvent GoalReachedEvent = default;
    [SerializeField] private TMP_Text TicketUIText = default;
    public static TicketWallet inst;
    [SerializeField]
    private int ticketCount;
    [SerializeField]
    private int ticketGoal;

    private void Start()
    {
        if (inst == null)
        {
            inst = this;
            if (GoalReachedEvent == null)
            {
                GoalReachedEvent = new UnityEvent();
            }
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
        if (ticketCount >= ticketGoal)
        {
            GoalReachedEvent.Invoke();
        }
        if (TicketUIText) TicketUIText.text = ticketCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player") && ticketCount >= ticketGoal)
        {
            Debug.Log("Play Final Cutscene");
        }
    }
}
