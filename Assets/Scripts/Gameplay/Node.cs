using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{
    [SerializeField] private TMP_Text UnitText;
    [SerializeField] public int UnitsCount;
    [SerializeField] public GameManager.Side NodeSide;
    [SerializeField] private Unit _unitPrefab;
    private float _spawnRadius;

    [SerializeField] private Material GreenMaterial;
    [SerializeField] private Material RedMaterial;
    [SerializeField] private Material WhiteMaterial;
    private Vector3 _defaultSacle;

    private void Start()
    {
        _defaultSacle = transform.localScale;
        _spawnRadius = GetComponent<SphereCollider>().radius / 2f;
        InitializeNode();
        SetRandomAnim();
    }

    public void InitializeNode()
    {

        if (NodeSide == GameManager.Side.Player)
            gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = GreenMaterial;

        if (NodeSide == GameManager.Side.AI)
            gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = RedMaterial;

        if (NodeSide == GameManager.Side.NotCaptured)
        {
            gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = WhiteMaterial;
            UnitsCount = 0;
        }

        SetUnitText(UnitsCount.ToString());
    }

    public void SetUnitText(string unitCount)
    {
        UnitText.text = unitCount;
    }

    private bool IsCapturedBySomeOne()
    {
        if(NodeSide != GameManager.Side.NotCaptured)
            return true;

        return false;
    }

    private void SetRandomAnim()
    {
        var randomRotation = new Vector3
        {
            x = UnityEngine.Random.Range(180f, 360f),
            y = UnityEngine.Random.Range(180f, 360f),
            z = UnityEngine.Random.Range(180f, 360f)
        };

        transform.GetChild(0).DORotate(randomRotation, 45f).SetEase(Ease.Linear).OnComplete(()=>SetRandomAnim());
        randomRotation = new Vector3
        {
            x = UnityEngine.Random.Range(180f, 360f),
            y = UnityEngine.Random.Range(180f, 360f),
            z = UnityEngine.Random.Range(180f, 360f)
        };

        transform.GetChild(1).DORotate(randomRotation,45f).SetEase(Ease.Linear).OnComplete(() => SetRandomAnim());
    }

    public void SendUnit(Node receiver)
    {
        if(receiver.NodeSide == NodeSide)
        {
            StartCoroutine(CreateUnitCorutine(UnitsCount / 2, receiver));
            UnitsCount /= 2;
            SetUnitText(UnitsCount.ToString());
            InitializeNode();
        }
        else
        {
            StartCoroutine(CreateUnitCorutine(UnitsCount, receiver));
            UnitsCount = 0;
            SetUnitText(UnitsCount.ToString());
            InitializeNode();
        }
    }

    private Unit CreateUnit()
    {
        var unitPosition = transform.position + UnityEngine.Random.insideUnitSphere * _spawnRadius;

        var unit = Instantiate(_unitPrefab, unitPosition, Quaternion.identity);
        unit.UnitSide = NodeSide;
        return unit;
    }

    private IEnumerator CreateUnitCorutine(int unitAmount, Node unitsTarget)
    {
        for (var i = 0; i < unitAmount; i++)
        {
            CreateUnit().MoveTo(unitsTarget);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f,0.4f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            var unit = other.GetComponent<Unit>();

            if (unit.Target != this)
                return;

            transform.DOScale(_defaultSacle + new Vector3(0.03f, 0.03f, 0.03f), 0.1f).OnComplete(() => transform.DOScale(_defaultSacle, 0.1f));
            if (NodeSide == unit.UnitSide)
            {
                UnitsCount++;
                SetUnitText(UnitsCount.ToString());
            }

            if(IsCapturedBySomeOne() && unit.UnitSide != NodeSide)
            {
                UnitsCount--;
                if(UnitsCount <= 0)
                {
                    NodeSide = GameManager.Side.NotCaptured;
                    InitializeNode();
                }

                SetUnitText(UnitsCount.ToString());
                Destroy(unit.gameObject);
                return;
            }

            if (NodeSide == GameManager.Side.NotCaptured)
            {
                NodeSide = unit.UnitSide;
                UnitsCount++;
                InitializeNode();
            }

            Destroy(unit.gameObject);
            GameManager.CheckAllNodes();
        }
    }
}
