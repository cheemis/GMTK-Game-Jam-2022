using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadOutOfCutscene : MonoBehaviour
{
    public int nextScene = 0;
    public int cutSceneTime = 18;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadNextScene", cutSceneTime);
    }

    private void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().name == "Cutscene 1") 
        {
            AudioManager.Instance.PlayArcadeAmbience();
          }
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
