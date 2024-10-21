using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject restartAlert;
    [SerializeField] private GameObject exitAlert;

    private GameObject rootUI;
    private GameObject childUI;

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        rootUI = pauseUI;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        rootUI = null;
    }

    public void ActivateRestartAlert()
    {
        rootUI.SetActive(false);
        restartAlert.SetActive(true);
        childUI = restartAlert;
    }

    public void ActivateExitAlert()
    {
        Time.timeScale = 0f;
        if (rootUI != null)
        {
            rootUI.SetActive(false);
        }
        exitAlert.SetActive(true);
        childUI = exitAlert;
    }

    public void BackToRootUI()
    {
        childUI.SetActive(false);
        if (rootUI != null)
        {
            rootUI.SetActive(true);
        } 
    }
}
