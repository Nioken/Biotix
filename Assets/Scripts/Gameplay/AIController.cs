using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Node _startNode;
    private Node _firstNode;
    private Node _secondNode;

    private void Start()
    {
        foreach (var Node in GameManager.AllNodes)
            if (Node.NodeSide == GameManager.Side.AI)
                _startNode = Node;

        StartCoroutine(DoFirstStep());
    }

    IEnumerator DoFirstStep()
    {
        Debug.Log("AI: I come to did first step!");
        yield return new WaitForSeconds(Random.Range(1,3));
        var NotCapturedNodes = ReturnNotCapturedNodes();
        _startNode.SendUnit(NotCapturedNodes[Random.Range(0, NotCapturedNodes.Count)]);
        Debug.Log("AI: I did it!");
        StartCoroutine(AIRandomStep());
    }

    IEnumerator AIRandomStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1,10f));
            FromRandomToRandomNodeStep();
        }
    }

    private void FromRandomToRandomNodeStep()
    {
        var aiNodes = ReturnAllAINodes();
        var notAINodes = ReturnAllWithoutAINodes();
        if (aiNodes != null && notAINodes != null)
        {
            _firstNode = aiNodes[Random.Range(0, aiNodes.Count)];
            _secondNode = notAINodes[Random.Range(0, notAINodes.Count)];
        }

        _firstNode.SendUnit(_secondNode);
    }

    private List<Node> ReturnNotCapturedNodes()
    {
        var nodes = from node in GameManager.AllNodes
                    where node.NodeSide == GameManager.Side.NotCaptured
                    select node;

        return nodes.ToList();
    }

    public static List<Node> ReturnAllAINodes()
    {
        var nodes = from node in GameManager.AllNodes
                    where node.NodeSide == GameManager.Side.AI
                    select node;

        return nodes.ToList();
    }

    private List<Node> ReturnAllWithoutAINodes()
    {
        var nodes = from node in GameManager.AllNodes
                    where node.NodeSide != GameManager.Side.AI
                    select node;

        return nodes.ToList();
    }
}
