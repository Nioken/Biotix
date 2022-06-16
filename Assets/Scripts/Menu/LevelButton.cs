using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public LevelConfig Config;
    public TMP_Text LevelNumberText;
    public Image LockImage;
    public GameObject CantPlayMessage;

    private void Start()
    {
        var button = GetComponent<Button>();
        if (Config.IsPassed)
        {
            button.targetGraphic.color = Color.green;
        }
        if (!Config.canPlay)
        {
            LockImage.gameObject.SetActive(true);
            LevelNumberText.gameObject.SetActive(false);
            button.onClick.AddListener(ShowCantPlayMessage);
        }
        else
        {
            button.onClick.AddListener(PlayThisLevel);
        }

    }

    private void PlayThisLevel()
    {
        AudioManager.PlayUISound();
        GameManager.Config = Config;
        LoadingScreen.GameSceneLoadOperation.allowSceneActivation = true;
    }

    private void ShowCantPlayMessage()
    {
        CantPlayMessage.SetActive(true);
    }
}
