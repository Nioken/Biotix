using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _pausePanel;
    [SerializeField] private Image _levelEndUI;
    [SerializeField] private TMP_Text _levelEndText;

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _replayButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private Button _endLevelReplayButton;
    [SerializeField] private Button _endLevelToMenuButton;

    private void Start()
    {
        Settings.LoadSettings();
        GameManager._uiManager = GetComponent<UIManager>();
        AddUIListeners();
    }

    private void AddUIListeners()
    {
        _pauseButton.onClick.AddListener(PauseGame);
        _continueButton.onClick.AddListener(UnpauseGame);
        _replayButton.onClick.AddListener(ReplayLevel);
        _toMenuButton.onClick.AddListener(ToMenu);
        _endLevelReplayButton.onClick.AddListener(ReplayLevel);
        _endLevelToMenuButton.onClick.AddListener(ToMenu);
    }

    private void PauseGame()
    {
        _pausePanel.gameObject.SetActive(true);
        _pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0;
        AudioManager.PlayUISound();
    }

    private void UnpauseGame()
    {
        _pausePanel.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1;
        AudioManager.PlayUISound();
    }

    private void ReplayLevel()
    {
        AudioManager.PlayUISound();
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

    private void ToMenu()
    {
        AudioManager.PlayUISound();
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }

    public void ShowLevelEndUI(bool isWin)
    {
        _levelEndUI.gameObject.SetActive(true);
        Time.timeScale = 0;
        if (isWin)
            _levelEndText.text = "Level passed.";
        else
            _levelEndText.text = "Try again.";
    }

    private void RemoveUIListeners()
    {
        _pauseButton.onClick.RemoveAllListeners();
        _continueButton.onClick.RemoveAllListeners();
        _replayButton.onClick.RemoveAllListeners();
        _toMenuButton.onClick.RemoveAllListeners();
        _endLevelReplayButton.onClick.RemoveAllListeners();
        _endLevelToMenuButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        RemoveUIListeners();
        Settings.SaveSettings();
    }
}
