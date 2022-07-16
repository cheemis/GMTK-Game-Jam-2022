using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Kick : MonoBehaviour
{
    private Vector3 lastFramePos;
    private Vector3 prevLastFramePos;


    // Start is called before the first frame update
    void Start()
    {
        prevLastFramePos = transform.position;
        lastFramePos = transform.position;
    }

    private void FixedUpdate()
    {
        prevLastFramePos = lastFramePos;
        lastFramePos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player") { return; }

        Vector3 velocity = (lastFramePos - prevLastFramePos) / Time.deltaTime;

        Vector3 force = Vector3.ClampMagnitude((velocity + Vector3.up * 5f) * 100f, 10000f);
        other.gameObject.GetComponent<DiceCharacter>().RagdollHit(force);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
