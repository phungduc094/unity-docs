using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private int bodyLength;
    [SerializeField] private GameObject headPref;
    [SerializeField] private GameObject bodyPref;

    private List<Transform> pieceList = new List<Transform>();
    private Movement m_movement;
    private Vector2Int m_moveDir;
    private Vector2Int m_headPosition;

    private void Update()
    {
        InputHandle();
    }

    private void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && m_moveDir != Vector2Int.up)
        {
            Move(Vector2Int.down, new Vector2Int(m_headPosition.x, 0));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && m_moveDir != Vector2Int.down)
        {
            Move(Vector2Int.up, new Vector2Int(m_headPosition.x, GridUtils.Height - 1));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && m_moveDir != Vector2Int.left)
        {
            Move(Vector2Int.left, new Vector2Int(0, m_headPosition.y));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && m_moveDir != Vector2Int.right)
        {
            Move(Vector2Int.right, new Vector2Int(GridUtils.Width - 1, m_headPosition.y));
        }

        void Move(Vector2Int dir, Vector2Int endPosition)
        {
            m_moveDir = dir;
            m_movement.Move(m_headPosition, endPosition, dir, pieceList, UpdateHeadPosition);
        }
    }

    public void Init()
    {
        m_movement = GetComponent<Movement>();

        int x = 0;
        int y = 0;

        // insert from last -> first
        for(int i = 0; i < bodyLength; i++)
        {
            Vector2 bodyPosition = GridUtils.Position(x, y);
            pieceList.Insert(0, Instantiate(bodyPref, bodyPosition, Quaternion.identity, transform).transform);
            m_movement.movePositionList.Insert(0, bodyPosition);

            x++;
        }

        m_headPosition = new Vector2Int(x, y);
        Vector2 headPosition = GridUtils.Position(x, y);
        pieceList.Insert(0, Instantiate(headPref, headPosition, Quaternion.identity, transform).transform);
        m_movement.movePositionList.Insert(0, headPosition);
    }

    private void UpdateHeadPosition(Vector2Int headPosition)
    {
        m_headPosition = headPosition;
    }
}
