using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSounds : MonoBehaviour
{
    public AudioClip footstep;
    public AudioClip jump;
    public AudioClip landing;
    public AudioClip bounce;

    private DiceCharacter owner;

    private bool groudedLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<DiceCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(owner.grounded)
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayFootstep();
    }

    public void PlayFootstep() 
    {
        AudioSource.PlayClipAtPoint(footstep, transform.position + Vector3.down, 10f);
    }

    public void PlayDiceCollision() 
    {
        PlayFootstep();
    }

    public void PlayJumpSound() 
    {
    }
}
