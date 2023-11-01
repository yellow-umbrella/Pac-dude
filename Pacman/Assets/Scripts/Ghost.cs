using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ghost : MonoBehaviour
{
    public enum Movement
    {
        Greedy = 0,
        AStar = 1
    }

    [SerializeField] private Vector2Int startCoordinates;
    [SerializeField] private float timeBetweenMoves;
    [SerializeField] private Movement movement;

    private Maze maze;
    private PlayerController player;
    private bool canMove = true;

    private void Start()
    {
        maze = transform.parent.gameObject.GetComponent<Maze>();
        player = maze.GetPlayer();
        transform.localPosition = ((Vector3Int)maze.Coord2LocalPos(startCoordinates));
    }

    private void Update()
    {
        Vector2Int currentPos = Vector2Int.RoundToInt(transform.localPosition);
        Vector2Int target = Vector2Int.RoundToInt(player.transform.localPosition);
        if (canMove)
        {
            if (currentPos == target)
            {
                maze.DamagePlayer();
            } 
            else if (movement == Movement.Greedy)
            {
                Vector2Int newPos = maze.FindGreedyMove(currentPos, target);
                transform.localPosition = ((Vector3Int)newPos);
            }
            else if (movement == Movement.AStar)
            {
                Vector2Int newPos = maze.FindAStarMove(currentPos, target);
                transform.localPosition = ((Vector3Int)newPos);
            }
            StartCoroutine(MoveTimer());
        }
    }

    private IEnumerator MoveTimer()
    {
        canMove = false;
        yield return new WaitForSeconds(timeBetweenMoves);
        canMove = true;
    }
}
