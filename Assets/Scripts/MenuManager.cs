using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _SettingButton;
    [SerializeField] private Button _recordsButton;
    [SerializeField] private Button _levelsBackButton;
    [Space]
    [Header("Window")]
    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private GameObject _levelsWindow;
    [SerializeField] private GameObject _settingsWindow;
    [SerializeField] private GameObject _recordsWindow;
    [Space]
    [SerializeField] private LineRenderer _lineRenderer;
    public LevelButton LevelButtonPrefab;
    public Transform LevelButtonsTransform;
    public List<LevelButton> LevelButtons = new List<LevelButton>();

    private void OnEnable()
    {
        LoadLevelsProgress();
        InitializeLevelButtons();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        AddUIListeners();
        SetLevelsLine();
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

    private void LoadLevelsProgress()
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

    private void AddUIListeners()
    {
        _playButton.onClick.AddListener(ShowLevelWindow);
        _levelsBackButton.onClick.AddListener(showMenuWindow);
    }

    public void ShowLevelWindow()
    {
        _menuWindow.SetActive(false);
        _levelsWindow.SetActive(true);
        AudioManager.PlayUISound();
    }

    public void showMenuWindow()
    {
        _levelsWindow.SetActive(false);
        _menuWindow.SetActive(true);
        AudioManager.PlayUISound();
    }

    public void SetLevelsLine()
    {
        for(int i = 0; i < LevelButtons.Count; i++)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, LevelButtons[i].transform.position);
        }
    }

    private void InitializeLevelButtons()
    {
        for(int i = 1; i < LevelButtons.Count; i++)
            if (LevelButtons[i - 1].Config.IsPassed)
                LevelButtons[i].Config.canPlay = true;
        else
                LevelButtons[i].Config.canPlay = false;
    }

    private void OnDestroy()
    {
        SaveLevelsProgress();
    }

}
