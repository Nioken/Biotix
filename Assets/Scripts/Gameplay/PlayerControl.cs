using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private LineRenderer _playerLine;
    private RaycastHit _hit;
    private Node _firstNode;
    private Node _secondNode;

    private void Update()
    {

        #region EditorInput

#if UNITY_EDITOR

        if (Input.GetMouseButton(0))
            PlayerInput();

        if (Input.GetMouseButtonUp(0))
            EndInput();

#endif

        #endregion

        #region AndroidInput

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began || Input.touches[0].phase == TouchPhase.Moved)
                PlayerInput();

            if(Input.touches[0].phase == TouchPhase.Ended)
                EndInput();
        }

#endif

        #endregion

        if (_firstNode != null)
            DrawPlayerLine(_firstNode.transform.position);
    }

    private void EndInput()
    {
        if(_firstNode != null && _secondNode != null)
            _firstNode.SendUnit(_secondNode);

        _playerLine.SetPosition(0, Vector3.zero);
        _playerLine.SetPosition(1, Vector3.zero);
        _firstNode = null;
        if (_secondNode != null)
            _secondNode.HideSelection();

        _secondNode = null;
    }

    private void PlayerInput()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            if (_hit.collider.CompareTag("Node"))
            {
                var castedNode = GameManager.ReturnNode(_hit.collider.gameObject);
                if (_firstNode == null)
                {
                    if (castedNode.NodeSide == GameManager.Side.Player)
                    {
                        _firstNode = castedNode;
                    }
                }
                else
                {
                    if (castedNode != _firstNode)
                    {
                        _secondNode = castedNode;
                        _secondNode.ShowSelection();
                    }
                }
            }
        }
        else
        {
            if(_firstNode != null)
            {
                if (_secondNode != null)
                    _secondNode.HideSelection();

                _secondNode = null;
            }
        }
    }

    private void DrawPlayerLine(Vector3 firstNodePostion)
    {
        if (_secondNode == null)
        {
            _playerLine.SetPosition(0, firstNodePostion);
            var secondDotPosition = new Vector3
            {
                x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                z = 0
            };
            _playerLine.SetPosition(1, secondDotPosition);
        }
        else
        {
            _playerLine.SetPosition(0, firstNodePostion);
            _playerLine.SetPosition(1, _secondNode.transform.position);
        }
    }
}
