using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using System;
using System.Threading;

[CustomEditor(typeof(LevelConfig))]
public class LevelEditor : Editor
{
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private MenuManager _menuManager;
    private static LevelConfig _editableConfig;

    public override void OnInspectorGUI()
    {
        LevelConfig config = (LevelConfig)target;
        _menuManager = FindObjectOfType<MenuManager>();
        if (EditorSceneManager.GetActiveScene().name == "MenuScene")
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);
            if (GUILayout.Button("Edit"))
                StartEdit(config);

            if (GUILayout.Button("Create New"))
                CreateNewLevel();
        }

        if(EditorSceneManager.GetActiveScene().name == "Editor") 
        { 
            if (config == _editableConfig)
            {
                base.OnInspectorGUI();
                GUILayout.Space(10);
                if (GUILayout.Button("Create node"))
                    CreateNode();

                if (GUILayout.Button("Save&Exit"))
                    SaveLevel(config);
            }
            else
                GUILayout.Label("Уже редактируется уровень: " + _editableConfig.name);
        }
    }

    public void StartEdit(LevelConfig config)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Editor.unity");
        _editableConfig = config;
        if (config.NodesInfo != null)
            for (var i = 0; i < config.NodesInfo.Length; i++)
                CreateNode(config.NodesInfo[i]);
    }

    public void CreateNode()
    {
        var node = Instantiate(_nodePrefab, Vector3.zero, Quaternion.identity);
        UnityEditor.Selection.activeGameObject = node.gameObject;
    }

    public void CreateNode(LevelConfig.NodeInfo info)
    {
        var node = Instantiate(_nodePrefab, info.Position, Quaternion.identity);
        node.transform.localScale = info.Scale;
        node.NodeSide = info.NodeSide;
        node.UnitsCount = info.StartUnits;
        node.InitializeNode();
    }

    public void SaveLevel(LevelConfig config)
    {
        var nodes = FindObjectsOfType<Node>();
        Array.Reverse(nodes);
        config.NodesInfo = new LevelConfig.NodeInfo[nodes.Length];
        for (var i = 0; i < nodes.Length; i++)
        {
            config.NodesInfo[i] = new LevelConfig.NodeInfo(nodes[i].transform.position,nodes[i].transform.localScale, nodes[i].NodeSide, nodes[i].UnitsCount);
            DestroyImmediate(nodes[i]);
        }
        EditorSceneManager.OpenScene("Assets/Scenes/MenuScene.unity");
    }

    public void CreateNewLevel()
    {
        LevelConfig newLevelConfig = CreateInstance<LevelConfig>();
        AssetDatabase.CreateAsset(newLevelConfig, "Assets/Resources/Configs/newLevelConfig.asset");
        AssetDatabase.SaveAssets();
        _menuManager.ShowLevelWindow();
        CreateNewLevelButton(newLevelConfig);
    }

    private void CreateNewLevelButton(LevelConfig config)
    {
        var button = Instantiate(_menuManager.LevelButtonPrefab, _menuManager.LevelButtonsTransform);
        button.Config = config;
        Selection.activeGameObject = button.gameObject;
        var levelNumber = FindObjectsOfType<LevelButton>().Length;
        button.LevelNumberText.text = levelNumber.ToString();
        _menuManager.LevelButtons.Add(button);
        AssetDatabase.RenameAsset("Assets/Configs/newLevelConfig.asset","Level " + levelNumber.ToString() + " Config");
        AssetDatabase.SaveAssets();
        button.name = "Level " + levelNumber.ToString();
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
}
