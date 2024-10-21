using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public float slowdownDuration = 2.0f;
    public static TimeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StopGameImmediately()
    {
        Time.timeScale = 0;
    }

    public void ResumeGameImmediately()
    {
        Time.timeScale = 1;
    }

    public IEnumerator SlowDownTime(Action afterSlowDown)
    {
        float elapsedTime = 0.0f;
        float initialTimeScale = Time.timeScale;
        while (elapsedTime < slowdownDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, 0.0f, elapsedTime / slowdownDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = 0.0f;
        afterSlowDown.Invoke();
    }

    public IEnumerator SpeedUpTime()
    {
        float elapsedTime = 0.0f;
        float initialTimeScale = Time.timeScale;
        while (elapsedTime < slowdownDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, 1.0f, elapsedTime / slowdownDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }
        Time.timeScale = 1.0f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}
