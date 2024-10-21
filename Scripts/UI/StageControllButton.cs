using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class StageControllButton : MonoBehaviour
{
    private Image buttonImage;
    private Button button;
    [SerializeField] private Sprite pauseSprite; 
    [SerializeField] private Sprite startSprite;
    [SerializeField] private UnityEvent startWave;
    [SerializeField] private UnityEvent pauseGame;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    public void ChangeToPauseMode()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PauseGame);
        buttonImage.sprite = pauseSprite;
    }

    public void ChangeToStartMode()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StartWave);
        buttonImage.sprite = startSprite;
    }

    public void PauseGame()
    {
        pauseGame.Invoke();
    }

    public void StartWave()
    {
        startWave.Invoke();
    }
}
