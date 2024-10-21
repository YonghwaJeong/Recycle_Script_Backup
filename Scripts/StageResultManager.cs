using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageResultManager : MonoBehaviour
{
    [SerializeField] private GameObject failedUI;
    [SerializeField] private GameObject clearUI;
    public void GameOver()
    {
        StartCoroutine(TimeManager.Instance.SlowDownTime(OnGameOver));
    }

    public void GameClear()
    {
        StartCoroutine(TimeManager.Instance.SlowDownTime(OnGameClear));
    }

    private void OnGameOver()
    {
        failedUI.SetActive(true);
    }

    public void OnGameClear()
    {
        clearUI.SetActive(true);
    }
}
