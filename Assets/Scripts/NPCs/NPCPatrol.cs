using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{

    //Components
    private Animator anim;
    private NavMeshAgent nav;


    //Patrolling Variables
    public Vector2 idleTimes = new Vector2(1, 10);
    private Vector3 dest;
    public Vector2 range = new Vector2(50, 100);


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        nav.avoidancePriority = Random.Range(0, 50);

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 50, NavMesh.AllAreas);
        nav.Warp(hit.position);

        if (Random.Range(0, 100) > 50)
        {
            Debug.Log("idle");
            StartCoroutine(Idle(Random.Range(idleTimes.x, idleTimes.y)));
        }
        else
        {
            Debug.Log("travel");
            SetNewDestination();
        }
    }

    private void Update()
    {
        anim.SetFloat("speed", nav.velocity.magnitude);
    }

    //this method sets a new destination for the NPC to wander to
    private void SetNewDestination()
    {
        NavMeshHit hit;

        Vector2 rand = Random.insideUnitCircle * Random.Range(range.x, range.y);
        Vector3 pos = new Vector3(rand.x, 0, rand.y);

        NavMesh.SamplePosition(pos, out hit, 50, nav.areaMask);

        dest = hit.position;
        Debug.Log(dest);
        Debug.Log(nav.SetDestination(dest));

        StartCoroutine(Travelling());
    }

    //this coroutine waits at a destination before moving to a new position
    IEnumerator Idle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetNewDestination();
    }

    //this coroutine checks if the NPC has reached it's destination
    IEnumerator Travelling()
    {
        while (Vector3.Distance(transform.position, dest) > 1 || nav.velocity.magnitude > .01f)
        {
            yield return 0;
        }
        yield return 0;
        StartCoroutine(Idle(Random.Range(idleTimes.x, idleTimes.y)));
    }
}
