using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void SelectLostCityLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void SelectFactoryLevel()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void SelectBeachLevel()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void SelectTestLevel()
    {
        SceneManager.LoadSceneAsync(5);
    }

    public void BacktoMain()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void BacktoSelect()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
