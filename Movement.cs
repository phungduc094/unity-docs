using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    [Header("Move config")]
    [SerializeField] private float speed = 0.08f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private Ease easeLast = Ease.OutBack;

    [Header("Read Only")]
    public List<Vector2> movePositionList = new List<Vector2>();

    public void Move(Vector2Int headPosition, Vector2Int endPosition, Vector2Int moveDir, List<Transform> pieceList, 
        Action<Vector2Int> updateHeadPosition)
    {
        movePositionList.RemoveAt(movePositionList.Count - 1);

        headPosition += moveDir;
        movePositionList.Insert(0, GridUtils.Position(headPosition.x, headPosition.y));

        // body move
        for (int i = 1; i < pieceList.Count; i++)
        {
            pieceList[i].DOMove(movePositionList[i], speed).SetEase(ease);
        }

        // head move
        pieceList[0].DOMove(movePositionList[0], speed).SetEase(ease).OnComplete(() => 
        {
            updateHeadPosition?.Invoke(headPosition);
            if (headPosition != endPosition)
            {
                Move(headPosition, endPosition, moveDir, pieceList, updateHeadPosition);
            }
        });
        
    }
}
