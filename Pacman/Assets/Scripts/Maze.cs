using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.GraphicsBuffer;

public class Maze : MonoBehaviour
{
    private int[,] maze =
    {
        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1, -1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1, 10, -1, -1,  1, -1, -1, -1,  1, -1,  1, -1, -1, -1,  1, -1, -1, 10, -1 },
        { -1,  1, -1, -1,  1, -1, -1, -1,  1, -1,  1, -1, -1, -1,  1, -1, -1,  1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1,  1, -1, -1,  1, -1,  1, -1, -1, -1, -1, -1,  1, -1,  1, -1, -1,  1, -1 },
        { -1,  1,  1,  1,  1, -1,  1,  1,  1, -1,  1,  1,  1, -1,  1,  1,  1,  1, -1 },
        { -1, -1, -1, -1,  1, -1, -1, -1,  0, -1,  0, -1, -1, -1,  1, -1, -1, -1, -1 },
        {  0,  0,  0, -1,  1, -1,  0,  0,  0,  0,  0,  0,  0, -1,  1, -1,  0,  0,  0 },
        { -1, -1, -1, -1,  1, -1,  0, -1, -1,  0, -1, -1,  0, -1,  1, -1, -1, -1, -1 },
        {  0,  0,  0,  0,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  0,  0,  0,  0 },
        { -1, -1, -1, -1,  1, -1,  0, -1, -1, -1, -1, -1,  0, -1,  1, -1, -1, -1, -1 },
        {  0,  0,  0, -1,  1, -1,  0,  0,  0,  0,  0,  0,  0, -1,  1, -1,  0,  0,  0 },
        { -1, -1, -1, -1,  1, -1,  0, -1, -1, -1, -1, -1,  0, -1,  1, -1, -1, -1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1, -1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1,  1, -1, -1,  1, -1, -1, -1,  1, -1,  1, -1, -1, -1,  1, -1, -1,  1, -1 },
        { -1, 10,  1, -1,  1,  1,  1,  1,  1,  0,  1,  1,  1,  1,  1, -1,  1, 10, -1 },
        { -1, -1,  1, -1,  1, -1,  1, -1, -1, -1, -1, -1,  1, -1,  1, -1,  1, -1, -1 },
        { -1,  1,  1,  1,  1, -1,  1,  1,  1, -1,  1,  1,  1, -1,  1,  1,  1,  1, -1 },
        { -1,  1, -1, -1, -1, -1, -1, -1,  1, -1,  1, -1, -1, -1, -1, -1, -1,  1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
    };

    private GameObject[,] mazeObjects = new GameObject[N, M];

    private const int N = 22;
    private const int M = 19;

    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject dot;
    [SerializeField] private List<Ghost> ghostPrefabs;
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private List<Vector2Int> cellsToGuard;

    private List<Ghost> ghosts;
    private PlayerController player;

    private int score = 0;
    private int lifes = 3;

    private int maxScore = 0;

    private void Start()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                if (maze[i, j] == -1)
                {
                    mazeObjects[i, j] = Instantiate(wall, transform);
                } else
                {
                    maxScore += maze[i, j];
                    mazeObjects[i, j] = Instantiate(dot, transform);
                    if (maze[i, j] == 0)
                    {
                        mazeObjects[i, j].GetComponent<SpriteRenderer>().sprite = null;
                    }
                    if (maze[i, j] > 1)
                    {
                        mazeObjects[i, j].GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                mazeObjects[i, j].transform.localPosition = ((Vector3Int)Coord2LocalPos(new Vector2Int(i, j)));
            }
        }

    }

    public void GenPlayer()
    {
        player = Instantiate(playerPrefab, transform);
        ghosts = new List<Ghost>();
        foreach (Ghost ghost in ghostPrefabs)
        {
            ghosts.Add(Instantiate(ghost, transform));
        }

    }

    private void Update()
    {
        if (score == maxScore)
        {
            Debug.Log("Victory!");
            GameManager.instance.Victory();
        }
    }

    public Vector2Int GetCellToGuard()
    {
        return Coord2LocalPos(cellsToGuard[Random.Range(0, cellsToGuard.Count)]);
    }

    public PlayerController GetPlayer()
    {
        return player;
    }

    public Vector2Int MoveLocalPos(Vector2Int currentPos, Vector2Int newPos)
    {
        return Coord2LocalPos(MoveCoord(LocalPos2Coord(currentPos), LocalPos2Coord(newPos)));
    }

    public Vector2Int MoveCoord(Vector2Int currentCoord, Vector2Int newCoord)
    {
        newCoord = new Vector2Int((N + newCoord.x) % N, (M + newCoord.y) % M);
        if (maze[newCoord.x, newCoord.y] != -1)
        {
            return newCoord;
        }
        return currentCoord;
    }

    public void CollectPoints(Vector2Int pos)
    {
        pos = LocalPos2Coord(pos);
        int points = maze[pos.x, pos.y];
        maze[pos.x, pos.y] = 0;
        mazeObjects[pos.x, pos.y].GetComponent<SpriteRenderer>().sprite = null;
        score += points;
    }

    public Vector2Int LocalPos2Coord(Vector2Int pos)
    {
        return new Vector2Int(-pos.y, pos.x);
    }

    public Vector2Int Coord2LocalPos(Vector2Int pos)
    {
        return new Vector2Int(pos.y, -pos.x);
    }

    public Vector2Int FindGreedyMove(Vector2Int pos, Vector2Int target)
    {
        pos = LocalPos2Coord(pos);
        target = LocalPos2Coord(target);
        Vector2Int newPos = pos;
        int minDist = int.MaxValue;
        List<Vector2Int> moves = new List<Vector2Int>()
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
        };
        foreach (Vector2Int move in moves)
        {
            Vector2Int potentialPos = pos + move;
            potentialPos = MoveCoord(pos, potentialPos);
            int dist = Dist(target, potentialPos);
            if (potentialPos != pos && dist < minDist)
            {
                minDist = dist;
                newPos = potentialPos;
            }
        }
        return Coord2LocalPos(newPos);
    }

    public Vector2Int FindRandomMove(Vector2Int pos)
    {
        pos = LocalPos2Coord(pos);
        var moves = GetValidMoves(pos);
        return Coord2LocalPos(moves[Random.Range(0, moves.Count)]);
    }

    public Vector2Int FindAStarMove(Vector2Int pos, Vector2Int target)
    {
        pos = LocalPos2Coord(pos);
        target = LocalPos2Coord(target);

        SortedDictionary<int, Queue<Vector2Int>> queue = new SortedDictionary<int, Queue<Vector2Int>>()
        {
            { 0, new Queue<Vector2Int>() }
        };
        queue[0].Enqueue(pos);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        bool foundPath = false;

        int[,] cost = new int[N, M];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                cost[i, j] = int.MaxValue;
            }
        }
        cost[pos.x, pos.y] = 0;

        while (queue.Count > 0)
        {
            var first = queue.First();
            var values = first.Value;
            Vector2Int current = values.Dequeue();
            if (values.Count == 0)
            {
                queue.Remove(first.Key);
            }

            if (current == target)
            {
                foundPath = true;
                break;
            }

            foreach (Vector2Int move in GetValidMoves(current))
            {
                int newCost = cost[current.x, current.y] + 1;
                if (newCost < cost[move.x, move.y])
                {
                    cost[move.x, move.y] = newCost;
                    int priority = newCost + Dist(move, target);
                    if (!queue.ContainsKey(priority))
                    {
                        queue[priority] = new Queue<Vector2Int>();
                    }
                    queue[priority].Enqueue(move);
                    cameFrom[move] = current;
                }
            }
            
        }

        if (!foundPath)
        {
            return Coord2LocalPos(pos);
        }

        Vector2Int cell = target;
        while (cameFrom[cell] != pos)
        {
            cell = cameFrom[cell];
        }
        return Coord2LocalPos(cell);
    }

    public int Dist(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> GetValidMoves(Vector2Int pos)
    {
        List<Vector2Int> moves = new List<Vector2Int>()
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
        };
        List<Vector2Int> res = new List<Vector2Int>();

        foreach (Vector2Int move in moves)
        {
            Vector2Int potentialPos = pos + move;
            potentialPos = MoveCoord(pos, potentialPos);
            if (potentialPos != pos)
            {
                res.Add(potentialPos);
            }
        }
        return res;
    }

    public void DamagePlayer()
    {
        lifes--;
        if (lifes == 0)
        {
            GameManager.instance.GameOver();
            Debug.Log("Game over!");
        }
    }

    public Vector2Int GetMazeSize()
    {
        return new Vector2Int(N, M);
    }

    public int GetLifes()
    {
        return lifes;
    }

    public int GetScore()
    {
        return score;
    }
}
