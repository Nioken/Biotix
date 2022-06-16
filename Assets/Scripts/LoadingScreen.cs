using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static bool IsLoaded;
    public static AsyncOperation GameSceneLoadOperation;
    public MenuManager MenuManager;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TMP_Text _maxUnitsText;
    [SerializeField] private TMP_Text _maxLevelTimeText;
    [SerializeField] private TMP_Text _minLevelTimeText;

    private void OnEnable()
    {
        Settings.LoadSettings();
        Debug.Log("Loaded");
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("MaxUnits"))
        {
            PlayerPrefs.SetInt("MaxUnits", 0);
            PlayerPrefs.SetFloat("MaxLevelTime", 0);
            PlayerPrefs.SetFloat("MinLevelTime", 0);
        }

        InitializeRecords();
        MenuManager.AddUIListeners();
        MenuManager.LoadLevelsProgress();
        MenuManager.InitializeLevelButtons();
        GameSceneLoadOperation = SceneManager.LoadSceneAsync("GameScene");
        GameSceneLoadOperation.allowSceneActivation = false;
    }

    private void InitializeRecords()
    {
        _maxUnitsText.text = "Max units in one node: " + PlayerPrefs.GetInt("MaxUnits");
        var minLevelMinutes = Mathf.RoundToInt(PlayerPrefs.GetFloat("MinLevelTime")) / 60;
        var maxLevelMinutes = Mathf.RoundToInt(PlayerPrefs.GetFloat("MaxLevelTime")) / 60;
        _maxLevelTimeText.text = "Max level time: " + maxLevelMinutes.ToString() + " min.";
        _minLevelTimeText.text = "Max level time: " + minLevelMinutes.ToString() + " min.";
    }

    private void Update()
    {
        if (IsLoaded)
            return;

        if (GameSceneLoadOperation == null)
            return;

        if (GameSceneLoadOperation.progress != 0.9f)
            _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, GameSceneLoadOperation.progress, 0.1f);
        else
            _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, 1, 0.1f);

        if (_progressBar.fillAmount >= 0.98f)
        {
            IsLoaded = true;
            HideLoadingScreen();
        }
    }

    public void HideLoadingScreen()
    {
        if (IsLoaded)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            MenuManager.showMenuWindow();
        }
    }
}
