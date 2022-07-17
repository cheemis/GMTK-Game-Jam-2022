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

    public GameObject blackFade;

    public void StartGame()
    {
        blackFade.SetActive(true);
        StartCoroutine(LoadGame());
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

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(1);
    }

}
