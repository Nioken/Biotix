using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    public GameManager.Side UnitSide;
    public Node Target;
    private Vector3 _moveTarget;

    private void Start()
    {
        InitializeUnit();
    }

    private void InitializeUnit()
    {
        if(UnitSide == GameManager.Side.Player)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = _greenMaterial;
            transform.GetChild(0).GetComponent<TrailRenderer>().material = _greenMaterial;
        }

        if (UnitSide == GameManager.Side.AI)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = _redMaterial;
            transform.GetChild(0).GetComponent<TrailRenderer>().material = _redMaterial;
        }

        transform.DOScale(Vector3.one, 0.2f);
    }

    public void MoveTo(Node target)
    {
        Target = target;
        _moveTarget = new Vector3
        {
            x = target.transform.position.x,
            y = target.transform.position.y,
            z = 1.6f
        };
    }

    private void Update()
    {
        if (Target == null)
            return;

        transform.LookAt(_moveTarget,Vector3.forward);
        transform.position = Vector3.MoveTowards(transform.position, _moveTarget, Time.deltaTime * GameManager.Config.UnitSpeed);        
    }
}
