using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        InitializeLevelButtons();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        AddUIListeners();
        SetLevelsLine();
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
    }

    public void showMenuWindow()
    {
        _levelsWindow.SetActive(false);
        _menuWindow.SetActive(true);
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
}
