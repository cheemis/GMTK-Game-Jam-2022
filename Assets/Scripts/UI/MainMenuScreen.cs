using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    //Menu Scenes
    public GameObject MainMenuObjects;
    public GameObject Settings;
    public GameObject Credits;

    private bool startedGame = false;
    public GameObject blackFade;

    public AudioManager audioManager;

    public void StartGame()
    {
        if(!startedGame)
        {
            blackFade.GetComponent<Animator>().SetTrigger("loadLevel");
            startedGame = true;
            blackFade.SetActive(true);
            if (SceneManager.GetActiveScene().name == "Level Test") 
        {
            AudioManager.Instance.PlayCutsceneMusic();
          }
            StartCoroutine(LoadGame());
        }
    }

    public void OpenSettings()
    {
        MainMenuObjects.SetActive(false);
        Settings.SetActive(true);
    }

    public void OpenCredits()
    {
        MainMenuObjects.SetActive(false);
        Credits.SetActive(true);
    }

    public void GoBack()
    {
        MainMenuObjects.SetActive(true);
        Settings.SetActive(false);
        Credits.SetActive(false);
    }

    public void LoadFromCharacterCreator()
    {
        blackFade.GetComponent<Animator>().SetTrigger("loadLevel");
      blackFade.SetActive(true);
    AudioManager.Instance.PlayCutsceneMusic();
    StartCoroutine(LoadNextSceneAgain());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

     IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

     IEnumerator LoadNextSceneAgain()
    {
        yield return new WaitForSeconds(2.1f);
        SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }


}
