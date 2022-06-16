using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private SpriteRenderer _background;
    public static LevelConfig Config;
    public static List<Node> AllNodes;
    public static UIManager ManagerUI;

    public enum Side
    {
        Player,
        AI,
        NotCaptured
    }

    private void Start()
    {
        _background.transform.DOShakeScale(10f, 1f, 2, 90, false).SetLoops(-1);
        CreateLevel();
        StartCoroutine(CountUnitCorutine());
    }

    private void CreateLevel()
    {
        AllNodes = new List<Node>();
        for (var i = 0; i < Config.NodesInfo.Length; i++)
            CreateNode(Config.NodesInfo[i]);
    }

    public void CreateNode(LevelConfig.NodeInfo info)
    {
        var node = Instantiate(_nodePrefab, info.Position, Quaternion.identity);
        node.transform.localScale = info.Scale;
        node.NodeSide = info.NodeSide;
        node.UnitsCount = info.StartUnits;
        AllNodes.Add(node);
    }

    private IEnumerator CountUnitCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Config.CreateSpeed);
            foreach (var Node in AllNodes)
            {
                if (Node.NodeSide != Side.NotCaptured)
                {
                    Node.UnitsCount++;
                    Node.SetUnitText(Node.UnitsCount.ToString());
                }
            }
        }
    }

    public static Node ReturnNode(GameObject nodeObject)
    {
        foreach(var Node in AllNodes)
            if(Node.gameObject == nodeObject)
                return Node;

        return null;
    }

    public static void CheckAllNodes()
    {
        var playerNodesCount = 0;
        var aiNodesCount = 0;
        foreach(var Node in AllNodes)
        {
            if (Node.NodeSide == Side.Player)
                playerNodesCount++;

            if (Node.NodeSide == Side.AI)
                aiNodesCount++;
        }

        if(playerNodesCount == 0)
            EndLevel(Side.AI);

        if(aiNodesCount == 0)
            EndLevel(Side.Player);
    }

    private static void EndLevel(Side winner)
    {
        if(winner == Side.Player)
        {
            Config.IsPassed = true;
            ManagerUI.ShowLevelEndUI(true);
        }
        else
            ManagerUI.ShowLevelEndUI(false);

        if(Time.timeSinceLevelLoad > PlayerPrefs.GetFloat("MaxLevelTime"))
            PlayerPrefs.SetFloat("MaxLevelTime", Time.timeSinceLevelLoad);

        if (Time.timeSinceLevelLoad < PlayerPrefs.GetFloat("MinLevelTime"))
            PlayerPrefs.SetFloat("MinLevelTime", Time.timeSinceLevelLoad);

        MenuManager.SaveLevelsProgress();
    }
}
