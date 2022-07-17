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
    private AudioSource soundPlayer;
    private bool groudedLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<DiceCharacter>();
        soundPlayer = this.GetComponent<AudioSource>();
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
        /*soundPlayer.clip = footstep;
        soundPlayer.pitch = 1f;
        soundPlayer.Play();*/
        AudioSource.PlayClipAtPoint(footstep, transform.position + Vector3.down, 10f);
    }

    public void PlayDiceCollision() 
    {
        PlayFootstep();
    }

    public void PlayJumpSound() 
    {
        soundPlayer.clip = jump;
        soundPlayer.pitch = Random.Range(0.5f, 1f);
        soundPlayer.volume = Random.Range(0.3f, 0.5f);
        soundPlayer.Play();

        
        //AudioSource.PlayClipAtPoint(jump, transform.position, 20f);
    }

    public void PlayLaunchSound()
    {
        soundPlayer.clip = jump;
        soundPlayer.pitch = Random.Range(0.6f, 1.1f);
        soundPlayer.volume = Random.Range(0.3f, 0.5f);
        soundPlayer.Play();


        //AudioSource.PlayClipAtPoint(jump, transform.position, 20f);
    }
}
