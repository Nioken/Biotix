using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _SettingButton;
    [SerializeField] private Button _recordsButton;
    [SerializeField] private Button _levelsBackButton;
    [SerializeField] private Button _sittingsButton;
    [SerializeField] private Button _settingsBackButton;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _soundsToggle;
    [SerializeField] private Button _cantPlayCloseButton;
    [SerializeField] private Button _gameExitButton;
    [SerializeField] private Button _hideExitWindowButton;
    [SerializeField] private Button _recordsBackButton;
    [Space]
    [Header("Window")]
    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private GameObject _levelsWindow;
    [SerializeField] private GameObject _settingsWindow;
    [SerializeField] private GameObject _recordsWindow;
    [SerializeField] private GameObject _cantPlayMessage;
    [SerializeField] private GameObject _exitWindow;
    [Space]
    [SerializeField] private LineRenderer _lineRenderer;
    public LevelButton LevelButtonPrefab;
    public Transform LevelButtonsTransform;
    public List<LevelButton> LevelButtons = new List<LevelButton>();
    [SerializeField] private LoadingScreen _loadingScreen;

    private void OnEnable()
    {
        if(LoadingScreen.IsLoaded)
            _loadingScreen.HideLoadingScreen();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        SetLevelsLine();
        _musicToggle.isOn = Settings.IsMusic;
        _soundsToggle.isOn = Settings.IsSounds;
    }

    public void Update()
    {
        if (_menuWindow.activeSelf == false)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
            ShowExitWindow();
    }

    public static void SaveLevelsProgress()
    {
        using (StreamWriter sr = new StreamWriter(Application.persistentDataPath + "/save.sav")) 
        {
            var configs = Resources.LoadAll<LevelConfig>("Configs/");
            string SerializedLevelConfigs = JsonConvert.SerializeObject(configs, Formatting.Indented);
            sr.Write(SerializedLevelConfigs);
        }
    }

    public void LoadLevelsProgress()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Application.persistentDataPath + "/save.sav"))
            {
                var loadedConfigs = JsonConvert.DeserializeObject<LevelConfig[]>(sr.ReadToEnd());
                for (var i = 0; i < loadedConfigs.Length; i++)
                {
                    LevelButtons[i].Config.canPlay = loadedConfigs[i].canPlay;
                    LevelButtons[i].Config.IsPassed = loadedConfigs[i].IsPassed;
                }
            }
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.Log("Save file is not created");
        }
    }

    public void AddUIListeners()
    {
        _playButton.onClick.AddListener(ShowLevelWindow);
        _levelsBackButton.onClick.AddListener(showMenuWindow);
        _SettingButton.onClick.AddListener(ShowSettingsWindow);
        _settingsBackButton.onClick.AddListener(showMenuWindow);
        _musicToggle.onValueChanged.AddListener((bool isOn) => MusicToggle());
        _soundsToggle.onValueChanged.AddListener((bool isOn) => SoundsToggle());
        _cantPlayCloseButton.onClick.AddListener(HideCantPlayMessage);
        _gameExitButton.onClick.AddListener(ExitFromGame);
        _hideExitWindowButton.onClick.AddListener(HideExitWindow);
        _recordsButton.onClick.AddListener(ShowRecordsWindow);
        _recordsBackButton.onClick.AddListener(showMenuWindow);
    }

    public void ShowLevelWindow()
    {
        _menuWindow.SetActive(false);
        _levelsWindow.SetActive(true);
        AudioManager.PlayUISound();
    }

    public void ShowSettingsWindow()
    {
        AudioManager.PlayUISound();
        _settingsWindow.SetActive(true);
        _menuWindow.SetActive(false);
    }

    public void showMenuWindow()
    {
        _recordsWindow.SetActive(false);
        _levelsWindow.SetActive(false);
        _settingsWindow.SetActive(false);
        _menuWindow.SetActive(true);
        AudioManager.PlayUISound();
    }

    public void MusicToggle()
    {
        AudioManager.PlayUISound();
        Settings.IsMusic = _musicToggle.isOn;
        Settings.SaveSettings();
        AudioManager.CheckSettings();
    }

    public void SoundsToggle()
    {
        AudioManager.PlayUISound();
        Settings.IsSounds = _soundsToggle.isOn;
        Settings.SaveSettings();
        AudioManager.CheckSettings();
    }

    public void SetLevelsLine()
    {
        for(int i = 0; i < LevelButtons.Count; i++)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, LevelButtons[i].transform.position);
        }
    }

    public void InitializeLevelButtons()
    {
        for (int i = 1; i < LevelButtons.Count; i++)
            if (LevelButtons[i - 1].Config.IsPassed)
            {
                LevelButtons[i].Config.canPlay = true;
                LevelButtons[i].CantPlayMessage = _cantPlayMessage;
            }
            else
            {
                LevelButtons[i].Config.canPlay = false;
                LevelButtons[i].CantPlayMessage = _cantPlayMessage;
            }
    }

    public void HideCantPlayMessage()
    {
        _cantPlayMessage.SetActive(false);
    }

    public void ShowExitWindow()
    {
        _exitWindow.SetActive(true);
    }

    public void HideExitWindow()
    {
        _exitWindow.SetActive(false);
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }

    public void ShowRecordsWindow()
    {
        _menuWindow.SetActive(false);
        _recordsWindow.SetActive(true);
    }

    private void OnDestroy()
    {
        SaveLevelsProgress();
    }

}
