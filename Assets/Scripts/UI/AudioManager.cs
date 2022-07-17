using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource ArcadeMusic;
    public AudioSource ArcadeAmbience;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


  
    // Start is called before the first frame update
    void Start()
    {
          DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCutsceneMusic()
    {
        ArcadeMusic.Play();
        ArcadeAmbience.Pause();
    }

    public void PlayArcadeAmbience()
    {
        ArcadeMusic.Stop();
        ArcadeAmbience.Play();
    }
}
