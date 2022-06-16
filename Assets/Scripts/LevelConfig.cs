using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName ="LevelConfig")]
[System.Serializable]
public class LevelConfig : ScriptableObject
{
    public bool canPlay;
    public bool IsPassed;
    [JsonIgnore] public float UnitSpeed;
    [JsonIgnore] public float CreateSpeed;
    [JsonIgnore] public NodeInfo[] NodesInfo;
    [JsonIgnore] public int MinAIThinkTime;
    [JsonIgnore] public int MaxAIThinkTime;

    [System.Serializable]
    public struct NodeInfo
    {
        public Vector3 Position;
        public Vector3 Scale;
        public GameManager.Side NodeSide;
        public int StartUnits;
        public NodeInfo(Vector3 position, Vector3 scale, GameManager.Side side, int startUnits)
        {
            Position = position;
            Scale = scale;
            NodeSide = side;
            StartUnits = startUnits;
        }
    }
}
