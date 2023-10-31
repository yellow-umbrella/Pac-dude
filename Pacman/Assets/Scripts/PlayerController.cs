using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Maze maze;
    [SerializeField] private Vector2Int startCoordinates;
    [SerializeField] private float timeBetweenMoves;

    private PlayerControls playerInput;
    private int score = 0;
    private bool canMove = true;

    private void Awake()
    {
        playerInput = new PlayerControls();
        playerInput.Player.Enable();
    }

    private void Start()
    {
        transform.localPosition = ((Vector3Int)maze.Coord2LocalPos(startCoordinates));
    }

    private void Update()
    {
        if (canMove)
        {
            Vector2Int currentPos = Vector2Int.RoundToInt(transform.localPosition);
            Vector2Int newPos = Vector2Int.RoundToInt(transform.localPosition) 
                + GetMovementInput();
            newPos = maze.Move(currentPos, newPos);
            if (newPos != currentPos)
            {
                transform.localPosition = (Vector3Int)newPos;
                score += maze.CollectPoints(newPos);
                StartCoroutine(MoveTimer());
            }
        }
    }

    private IEnumerator MoveTimer()
    {
        canMove = false;
        yield return new WaitForSeconds(timeBetweenMoves);
        canMove = true;
    }

    private Vector2Int GetMovementInput()
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        if (Mathf.Abs(inputVector.x) > Mathf.Abs(inputVector.y))
        {
            inputVector.y = 0;
        }
        else
        {
            inputVector.x = 0;
        }

        return Vector2Int.RoundToInt(inputVector.normalized);
    }
}
