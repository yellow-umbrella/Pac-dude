using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    private int[,] maze =
    {
        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1, -1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1,  1, -1, -1,  1, -1, -1, -1,  1, -1,  1, -1, -1, -1,  1, -1, -1,  1, -1 },
        { -1,  1, -1, -1,  1, -1, -1, -1,  1, -1,  1, -1, -1, -1,  1, -1, -1,  1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1,  1, -1, -1,  1, -1,  1, -1, -1, -1, -1, -1,  1, -1,  1, -1, -1,  1, -1 },
        { -1,  1,  1,  1,  1, -1,  1,  1,  1, -1,  1,  1,  1, -1,  1,  1,  1,  1, -1 },
        { -1, -1, -1, -1,  1, -1, -1, -1,  0, -1,  0, -1, -1, -1,  1, -1, -1, -1, -1 },
        {  0,  0,  0, -1,  1, -1,  0,  0,  0,  0,  0,  0,  0, -1,  1, -1,  0,  0,  0 },
        { -1, -1, -1, -1,  1, -1,  0, -1, -1,  0, -1, -1,  0, -1,  1, -1, -1, -1, -1 },
        {  0,  0,  0,  0,  1,  0,  0, -1,  0,  0,  0, -1,  0,  0,  1,  0,  0,  0,  0 },
        { -1, -1, -1, -1,  1, -1,  0, -1, -1, -1, -1, -1,  0, -1,  1, -1, -1, -1, -1 },
        {  0,  0,  0, -1,  1, -1,  0,  0,  0,  0,  0,  0,  0, -1,  1, -1,  0,  0,  0 },
        { -1, -1, -1, -1,  1, -1,  0, -1, -1, -1, -1, -1,  0, -1,  1, -1, -1, -1, -1 },
        { -1,  1,  1,  1,  1,  1,  1,  1,  1, -1,  1,  1,  1,  1,  1,  1,  1,  1, -1 },
        { -1,  1, -1, -1,  1, -1, -1, -1,  1, -1,  1, -1, -1, -1,  1, -1, -1,  1, -1 },
        { -1,  1,  1, -1,  1,  1,  1,  1,  1,  0,  1,  1,  1,  1,  1, -1,  1,  1, -1 },
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

    private void Start()
    {
        float scale = transform.localScale.x;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                if (maze[i, j] == -1)
                {
                    mazeObjects[i, j] = Instantiate(wall, transform);
                } else
                {
                    mazeObjects[i, j] = Instantiate(dot, transform);
                    if (maze[i, j] == 0)
                    {
                        mazeObjects[i, j].GetComponent<SpriteRenderer>().sprite = null;
                    }
                }
                mazeObjects[i, j].transform.localPosition = ((Vector3Int)Coord2LocalPos(new Vector2Int(i, j)));
            }
        }
    }

    public bool CanMove(Vector2Int pos)
    {
        pos = LocalPos2Coord(pos);
        if (maze[pos.x, pos.y] != -1)
        {
            return true;
        }
        return false;
    }

    public Vector2Int Move(Vector2Int current, Vector2Int pos)
    {
        pos = LocalPos2Coord(pos);
        pos = new Vector2Int((N + pos.x) % N, (M + pos.y) % M);
        if (maze[pos.x, pos.y] != -1)
        {
            return Coord2LocalPos(pos);
        }
        return current;
    }

    public int CollectPoints(Vector2Int pos)
    {
        pos = LocalPos2Coord(pos);
        int points = maze[pos.x, pos.y];
        maze[pos.x, pos.y] = 0;
        mazeObjects[pos.x, pos.y].GetComponent<SpriteRenderer>().sprite = null;
        return points;
    }

    public List<Vector2> FindPath()
    {
        return new List<Vector2>();
    }

    public Vector2Int LocalPos2Coord(Vector2Int pos)
    {
        return new Vector2Int(-pos.y, pos.x);
    }

    public Vector2Int Coord2LocalPos(Vector2Int pos)
    {
        return new Vector2Int(pos.y, -pos.x);
    }
}
