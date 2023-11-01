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
        AStar = 1,
        Protective = 2,
    }

    [SerializeField] private Vector2Int startCoordinates;
    [SerializeField] private float timeBetweenMoves;
    [SerializeField] private Movement movement;

    private Maze maze;
    private PlayerController player;
    private bool canMove = false;

    private Vector2Int cellToGuard;
    private int maxDist = 4;

    private void Start()
    {
        maze = transform.parent.gameObject.GetComponent<Maze>();
        player = maze.GetPlayer();
        transform.localPosition = ((Vector3Int)maze.Coord2LocalPos(startCoordinates));

        cellToGuard = maze.GetCellToGuard();
        StartCoroutine(MoveTimer());
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
            else
            {
                Vector2Int newPos = new Vector2Int();
                switch (movement)
                {
                    case Movement.Greedy:
                        newPos = maze.FindGreedyMove(currentPos, target);
                    break;
                    case Movement.AStar:
                        newPos = maze.FindAStarMove(currentPos, target);
                    break;
                    case Movement.Protective:
                        if (maze.Dist(cellToGuard, currentPos) > maxDist)
                        {
                            newPos = maze.FindAStarMove(currentPos, cellToGuard);
                        } else
                        {
                            newPos = maze.FindRandomMove(currentPos);
                        }
                    break;
                }
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
